using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using vmm.api.Models;

namespace vmm.api.Services
{
    public class ContoursService : IContoursManager
    {
        public static readonly MCvScalar CONTOUR_COLOR = new MCvScalar(0, 0, 255);
        public const int CONTOUR_THICKNESS = 3;

        public Shape Detect(string filename, string targetFilename)
        {
            return Detect(filename, targetFilename, 30.0, 150.0);
        }

        public Shape Detect(string filename, string targetFilename, double cannyThreshold, double cannyThresholdLinking)
        {
            return GetShape(cannyThreshold, cannyThresholdLinking, filename, targetFilename);
        }

        private Shape GetShape(double cannyThreshold, double cannyThresholdLinking, string filename, string targetFilename)
        {
            var image = CreateImage(filename);
            var cannyEdges = new UMat();
            CvInvoke.Canny(image, cannyEdges, cannyThreshold, cannyThresholdLinking);

            var contourImage = new Mat(image.Size, DepthType.Cv8U, 3);
            var contour = FindContour(cannyEdges, contourImage);

            var array = new List<PointF>();
            foreach (var p in contour.ToArray())
            {
                array.Add(new PointF(p.X, p.Y));
            }

            contourImage.Save(targetFilename);
            var center = GetCenter(contour);

            return new Shape()
            {
                Timeline = ShiftToStart(Normalize(ComputeDistancesFromCenterPoint(contour, center), 10)),
                Center = center,
                Points = contour.ToArray(),
                CannyTreshodLinking = cannyThresholdLinking,
                CannyTreshold = cannyThreshold,
            };
        }

        private IEnumerable<double> ShiftToStart(Dictionary<int, double> input)
        {
            var result = new Dictionary<int, double>();
            var maxItem = input.FirstOrDefault(x => x.Value == input.Values.Max()).Key;
            var k = 0;
            for (int i = maxItem; i < input.Count; i++)
            {
                result[k++] = input[i];
            }
            for (int i = 0; i < maxItem; i++)
            {
                result[k++] = input[i];
            }

            return result.Values.ToList();
        }

        private Dictionary<int, double> Normalize(Dictionary<int, double> input, int max)
        {
            var result = new Dictionary<int, double>();
            var ratio = max / input.Max(x => x.Value);
            foreach (var x in input)
            {
                result[x.Key] = x.Value * ratio;
            }
            return result;
        }

        private UMat CreateImage(string filename)
        {
            var image = new Image<Bgr, byte>(filename).Resize(1500, 1500, Inter.Linear, true);

            // Convert the image to grayscale and filter out the noise
            var result = new UMat();
            var nImage = new UMat();

            result = image.ToUMat();
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(result, nImage, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

            var erodeImg = new UMat();
            var dilateImg = new UMat();

            CvInvoke.Erode(nImage, erodeImg, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            CvInvoke.Dilate(erodeImg, dilateImg, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

            // Use image pyr to remove noise
            dilateImg.Save(filename + ".pic.jpeg");
            return dilateImg;
        }

        private VectorOfPoint FindContour(IInputOutputArray cannyEdges, IInputOutputArray result)
        {
            var largestIndex = 0;
            var largestArea = 0.0;
            VectorOfPoint largestContour = null;

            using (var hierachy = new Mat())
            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone);

                for (var i = 0; i < contours.Size; i++)
                {
                    var currentArea = CvInvoke.ContourArea(contours[i], false);
                    if (currentArea > largestArea)
                    {
                        largestArea = currentArea;
                        largestIndex = i;
                    }
                }

                CvInvoke.DrawContours(result, contours, largestIndex, new MCvScalar(0, 0, 255), 3, LineType.EightConnected, hierachy);
                largestContour = new VectorOfPoint(contours[largestIndex].ToArray());
            }
            return largestContour;
        }

        private MCvPoint2D64f GetCenter(VectorOfPoint contour)
        {
            var moment = CvInvoke.Moments(contour, true);
            return moment.GravityCenter;
        }

        private Dictionary<int, double> ComputeDistancesFromCenterPoint(VectorOfPoint contour, MCvPoint2D64f center)
        {
            var result = new Dictionary<int, double>();
            var index = 0;
            foreach (var c in contour.ToArray())
            {
                var x = center.X - c.X;
                var y = center.Y - c.Y;
                var distance = Math.Sqrt(x * x + y * y);
                result.Add(index++, distance);
            }
            return result;
        }

        public Result BestDTW(Shape s1, Shape s2, int? turns, int? w)
        {
            if (turns == null || turns == 0) return DynamicTimeWarping(s1, s2, w);
            Queue<double> q = new Queue<double>(s1.Timeline);

            var n = s1.Timeline.Count();
            var m = s2.Timeline.Count();
            var selectedTurningShape = s1;

            Result bestResult = new Result()
            {
                score = double.MaxValue,
                similarity = double.MaxValue
            };

            for (int i = 0; i < n; i += turns.Value)
            {
                for (int j = ((i != 0) ? i - turns.Value : i); j < i; j++)
                {
                    q.Enqueue(q.Dequeue());
                }
                selectedTurningShape.Timeline = q.ToArray();
                var r = DynamicTimeWarping(selectedTurningShape, s2, w);
                if (r.score < bestResult.score)
                {
                    bestResult = r;
                }
            }

            return bestResult;
        }

        public Result DynamicTimeWarping(Shape s1, Shape s2, int? w)
        {
            if (w.HasValue && w.Value == 0) w = null;
            var n = s1.Timeline.Count();
            var m = s2.Timeline.Count();
            var result = new double[n + 1, m + 1];

            if (w.HasValue)
            {
                w = Math.Max(w.Value, Math.Abs(n - m));
                for (var i = 0; i <= n; i++)
                    for (var j = 0; j <= m; j++)
                        result[i, j] = double.MaxValue;
            }
            else
            {
                for (var i = 1; i <= n; i++) result[i, 0] = 10000.0;
                for (var i = 1; i <= m; i++) result[0, i] = 10000.0;
            }
            result[0, 0] = 0;
            for (var i = 1; i <= n; i++)
                for (var j = ((w.HasValue) ? Math.Max(1, i - w.Value) : 1); j <= ((w.HasValue) ? Math.Min(m, i + w.Value) : m); j++)
                {
                    var cost = Math.Abs(s1.Timeline.ElementAt(i - 1) - s2.Timeline.ElementAt(j - 1));
                    result[i, j] = cost + min(result[i - 1, j], result[i, j - 1], result[i - 1, j - 1]);
                }
            var res = Backtrack(result, n, m);

            var distances = new List<double>();
            var dist2 = new List<Point>();
            var dist4 = new Dictionary<int, double>();
            var pX = (double)res.ElementAt(0).Y / (double)res.ElementAt(0).X;
            var k = 0;
            foreach (var x in res)
            {
                var origValue = (m / n) * x.X;
                var myValue = x.Y;

                distances.Add(Math.Abs(origValue - myValue));
                dist4.Add(k++, (Math.Abs(origValue - myValue)));
            }
            var similarity = distances.Sum() / res.Count();
            var max = distances.Max();
            var mm = distances.Min();

            var dist3 = Normalize(dist4, 10).Select(x => x.Value).ToList();
            var sim = dist3.Sum() / res.Count();

            var myResult = new Result()
            {
                result = res,
                score = result[n, m],
                similarity = similarity
            };
            return myResult;
        }

        private double min(double v1, double v2, double v3)
        {
            var v = Math.Min(v1, v2);
            return Math.Min(v, v3);
        }

        private IEnumerable<Point> Backtrack(double[,] costs, int n, int m)
        {
            var path = new List<Point>();
            var i = n - 1;
            var j = m - 1;
            while (i > 0 && j > 0)
            {
                var comp = min(costs[i - 1, j - 1], costs[i - 1, j], costs[i, j - 1]);
                if (i == 0) j = j - 1;
                else if (j == 0) i = i - 1;
                else
                {
                    if (costs[i - 1, j] == comp) i = i - 1;
                    else if (costs[i, j - 1] == comp) j = j - 1;
                    else
                    {
                        i = i - 1;
                        j = j - 1;
                    }
                }
                path.Add(new Point(j, i));
            }
            path.Add(new Point(0, 0));
            return path;
        }
    }
}
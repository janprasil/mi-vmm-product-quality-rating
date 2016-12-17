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
            contourImage.SetTo(new MCvScalar(0));
            var contour = FindContour(cannyEdges, contourImage);

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
            int k = 0;
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
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Gray);

            // Use image pyr to remove noise
            var pyrDown = new UMat();
            CvInvoke.PyrDown(result, pyrDown);
            CvInvoke.PyrUp(pyrDown, result);
            result.Save(filename + ".pic.jpeg");
            return result;
        }

        private VectorOfPoint FindContour(IInputOutputArray cannyEdges, IInputOutputArray result)
        {
            var largestIndex = 0;
            var largestArea = 0.0;
            VectorOfPoint largestContour = null;

            using (var hierachy = new Mat())
            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, hierachy, RetrType.List, ChainApproxMethod.ChainApproxNone);

                for (var i = 0; i < contours.Size; i++)
                {
                    var currentArea = CvInvoke.ContourArea(contours[i], false);
                    if (currentArea > largestArea)
                    {
                        largestArea = currentArea;
                        largestIndex = i;
                    }
                }

                CvInvoke.DrawContours(result, contours, largestIndex, new MCvScalar(0, 0, 255), 3, LineType.AntiAlias, hierachy);
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

        public Result BestDTW(Shape s1, Shape s2, int? turns)
        {
            if (turns == null || turns == 0) return DynamicTimeWarping(s1, s2);
            Queue<double> q = new Queue<double>(s1.Timeline);

            int n = s1.Timeline.Count();
            int m = s2.Timeline.Count();
            var selectedTurningShape = s1;

            Result bestResult = new Result()
            {
                score = double.MaxValue,
                similarity = double.MaxValue
            };

            for (int i = 0; i < n; i+=turns.Value)
            {
                for (int j = ((i != 0) ? i - turns.Value : i); j < i; j++)
                {
                    q.Enqueue(q.Dequeue());
                }
                selectedTurningShape.Timeline = q.ToArray();
                var r = DynamicTimeWarping(selectedTurningShape, s2);
                if (r.score < bestResult.score) bestResult = r;
            }

            return bestResult;
        }

        public Result DynamicTimeWarping(Shape s1, Shape s2)
        {
            int n = s1.Timeline.Count();
            int m = s2.Timeline.Count();
            var result = new double[n + 1, m + 1];
            for (var i = 1; i <= n; i++) result[i, 0] = 10000.0;
            for (var i = 1; i <= m; i++) result[0, i] = 10000.0;
            result[0, 0] = 0;
            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var cost = Math.Pow(s1.Timeline.ElementAt(i - 1) - s2.Timeline.ElementAt(j - 1), 2.0);
                    result[i, j] = cost + min(result[i - 1, j], result[i, j - 1], result[i - 1, j - 1]);
                }
            }
            var res = Backtrack(result, n, m);

            //y-x(m/n) = 0
            var distances = new List<double>();
            var pX = (double)res.ElementAt(0).Y / (double) res.ElementAt(0).X;
            foreach (var x in res)
            {
                distances.Add((Math.Abs(-1.0 * res.ElementAt(0).Y * x.X + res.ElementAt(0).X * x.Y) / Math.Sqrt(res.ElementAt(0).X * res.ElementAt(0).X + res.ElementAt(0).Y * res.ElementAt(0).Y)));
            }
            var similarity = distances.Sum() / res.Count();
            var max = distances.Max();
            var mm = distances.Min();
            //var similarity = res.Select(x => Math.Abs(x.X - x.Y)).Sum(x => x) / res.Count();
            return new Models.Result()
            {
                result = res,
                score = result[n, m],
                similarity = similarity
            };
        }

        private double min(double v1, double v2, double v3)
        {
            double v = Math.Min(v1, v2);
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
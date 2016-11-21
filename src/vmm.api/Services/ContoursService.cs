using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;
using vmm.api.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vmm.api.Services
{
    public class ContoursService : IContoursManager
    {
     
        private UMat _image;
        public static MCvScalar CONTOUR_COLOR = new MCvScalar(0, 0, 255);
        public static int CONTOUR_THICKNESS = 3;


        public Shape Detect(String filename, String targetFilename)
        {
            Shape s = null;
            // There is a big mess because I wanted to try best tresholds...
            //for( double i = 5.0; i<200.0; i+=5.0 )
            //{
            //    for (double j = 5.0; j<200.0; j+=5.0)
            //    {
                    double cannyThreshold = 30.0;
                    double cannyThresholdLinking = 150.0;
                    s = GetShape(cannyThreshold, cannyThresholdLinking, filename, targetFilename);
            //   }
            //}
            return s;
            
        }

        private Shape GetShape(double cannyThreshold, double cannyThresholdLinking, string filename, string targetFilename)
        {
            CreateImage(filename);
            UMat cannyEdges = new UMat();
            if (_image == null) return null;
            CvInvoke.Canny(_image, cannyEdges, cannyThreshold, cannyThresholdLinking);

            Mat contourImage = new Mat(_image.Size, DepthType.Cv8U, 3);
            contourImage.SetTo(new MCvScalar(0));
            var contour = FindContour(cannyEdges, contourImage);

            contourImage.Save(targetFilename/* + "-" + cannyThreshold.ToString() + "--" + cannyThresholdLinking.ToString() + ".jpeg"*/);
            var center = GetCenter(contour);

            return new Shape()
            {
                ImageUrl = targetFilename,
                Timeline = ShiftToStart(Normalize(ComputeDistancesFromCenterPoint(contour, center), 10)),
                Center = center,
                Points = contour.ToArray()
            };
        }

        private Dictionary<int, double> ShiftToStart(Dictionary<int, double> input)
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

            return result;
        }

        private Dictionary<int, double> Normalize(Dictionary<int, double> input, int max)
        {
            var result = new Dictionary<int, double>();
            var ratio = (double)max / input.Max(x => x.Value);
            foreach (var x in input)
            {
                result[x.Key] = x.Value * ratio;
            }
            return result;
        }
        private void CreateImage(String filename)
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(filename).Resize(1500, 1500, Emgu.CV.CvEnum.Inter.Linear, true);

            //Convert the image to grayscale and filter out the noise
            UMat result = new UMat();
            //result = image.ToUMat();
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(result, pyrDown);
            CvInvoke.PyrUp(pyrDown, result);
            result.Save(filename + ".hovno.jpeg");
            _image = result;
        }
        private VectorOfPoint FindContour(IInputOutputArray cannyEdges, IInputOutputArray result)
        {

            int             largestIndex    = 0;
            double          largestArea     = 0;
            VectorOfPoint   largestContour         = null;

            using (Mat hierachy = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, hierachy, RetrType.List, ChainApproxMethod.ChainApproxNone);

                for (int i = 0; i < contours.Size; i++)
                {
                    double currentArea = CvInvoke.ContourArea(contours[i], false);
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
        private MCvPoint2D64f GetCenter( VectorOfPoint contour )
        {
            var moment = CvInvoke.Moments(contour, true);
            return moment.GravityCenter;
        }
        private Dictionary<int, double> ComputeDistancesFromCenterPoint( VectorOfPoint contour, MCvPoint2D64f center )
        {
            var result = new Dictionary<int, double>();
            var index = 0;
            foreach (var c in contour.ToArray())
            {
                double x = center.X - c.X;
                double y = center.Y - c.Y;
                double distance = Math.Sqrt(x * x + y * y);
                result.Add(index++, distance);
            }
            return result;
        }
    }
}

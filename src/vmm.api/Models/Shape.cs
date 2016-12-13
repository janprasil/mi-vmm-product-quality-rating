using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;

namespace vmm.api.Models
{
    public class Shape
    {
        public string ImageUrl { get; set; }
        public string ContourImageUrl { get; set; }
        public string LocalPath { get; set; }
        public string ContourLocalPath { get; set; }
        public double CannyTreshold { get; set; }
        public double CannyTreshodLinking { get; set; }
        public IEnumerable<double> Timeline { get; set; }
        public IEnumerable<Point> Points { get; set; }
        public MCvPoint2D64f Center { get; set; }
        public bool Approved { get; set; }
    }
}
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;

namespace vmm.api.Models
{
    public class Shape
    {
        public string ImageUrl { get; set; }
        public Dictionary<int, double> Timeline { get; set; }
        public IEnumerable<Point> Points { get; set; }
        public MCvPoint2D64f Center { get; set; }
    }
}
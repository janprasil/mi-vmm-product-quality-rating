using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace vmm.api.Models
{
    public class Shape
    {
        public String ImageUrl { get; set; }
        public Dictionary<int, double> Timeline { get; set; }
        public IEnumerable<Point> Points { get; set; }
        public MCvPoint2D64f Center { get; set; }
    }
}

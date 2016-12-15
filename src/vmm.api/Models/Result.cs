using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace vmm.api.Models
{
    public class Result : IModel    
    {
        public string referenceId { get; set; }
        public string imageId { get; set; }
        public IEnumerable<Point> result { get; set; }
    }
}

﻿using System.Collections.Generic;
using System.Drawing;
using vmm.api.Models;

namespace vmm.api.Services
{
    public interface IContoursManager
    {
        Shape Detect(string filename, string targetFilename);
        IEnumerable<Point> DynamicTimeWarping(Shape s1, Shape s2);
    }
}
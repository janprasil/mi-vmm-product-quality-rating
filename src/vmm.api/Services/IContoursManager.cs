using System.Collections.Generic;
using vmm.api.Models;

namespace vmm.api.Services
{
    public interface IContoursManager
    {
        Shape Detect(string filename, string targetFilename);
        double DynamicTimeWarping(Shape s1, Shape s2);
    }
}
using System.Collections.Generic;
using System.Drawing;
using vmm.api.Models;

namespace vmm.api.Services
{
    public interface IContoursManager
    {
        Shape Detect(string filename, string targetFilename, double ct, double ctl);

        Shape Detect(string filename, string targetFilename);

        Result BestDTW(Shape s1, Shape s2, int? turns);

        Result DynamicTimeWarping(Shape s1, Shape s2);
    }
}
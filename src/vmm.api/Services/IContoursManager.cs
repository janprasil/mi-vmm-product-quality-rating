using vmm.api.Models;

namespace vmm.api.Services
{
    public interface IContoursManager
    {
        Shape Detect(string filename, string targetFilename);
    }
}
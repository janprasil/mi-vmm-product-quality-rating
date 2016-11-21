using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vmm.api.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vmm.api.Services
{
    public interface IContoursManager
    {
        Shape Detect(String filename, String targetFilename);
    }
}

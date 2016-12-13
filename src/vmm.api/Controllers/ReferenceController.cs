using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vmm.api.Controllers
{
    [Route("api/reference")]
    public class ReferenceController : Controller
    {
        [HttpPost]
        public JsonResult Post()
        {
            /*if (Request.Form.Files.Count == 0) return Json(new { Id = "NO_IMAGE", Message = "Upload an image" });
            var file = Request.Form.Files[0];
            var path = appEnvironment.ContentRootPath;
            var filename = $@"{path}\uploads\{file.FileName}";
            var contoursFilename = $"{file.FileName}.contures.jpeg";
            var target = $@"{path}\wwwroot\uploads\{contoursFilename}";
            var urlTarget = $"{HttpContext.Request.Host}/uploads/{contoursFilename}";

            using (var fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var result = contoursManager.Detect(filename, target);

                result.ImageUrl = urlTarget;
                list.Add(result);
            }
            */
            return Json(string.Empty);
        }
    }
}

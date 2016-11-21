using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api")]
    public class ContoursController : Controller
    {
        private IContoursManager _contoursManager;
        private readonly IHostingEnvironment _appEnvironment;

        public ContoursController(IContoursManager contoursManager, IHostingEnvironment appEnvironment)
        {
            _contoursManager = contoursManager;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Content("Create a POST Request with parameters ´files´");
        }

        [HttpPost]
        public ActionResult Post(IList<IFormFile> files)
        {
            var content = "";
            var list = new List<Shape>();
            foreach (var file in files)
            {
                var path = _appEnvironment.ContentRootPath;
                var filename = $@"{path}\upload\{file.FileName}";
                var contoursFilename = $"{file.FileName}.contures.jpeg";
                var target = $@"{path}\wwwroot\upload\{contoursFilename}";
                var urlTarget = $"{HttpContext.Request.Host}/upload/{contoursFilename}";

                using (var fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var result = _contoursManager.Detect(filename, target);
                result.ImageUrl = urlTarget;
                list.Add(result);
                //foreach (KeyValuePair<int, double> x in result)
                //{
                //    content += $"{x.Key.ToString()}\t{x.Value.ToString()}\n";
                //}
            }

            int maxSize = 0;
            foreach (var x in list)
            {
                if (x.Timeline.Count > maxSize) maxSize = x.Timeline.Count;
            }

            for (var i = 0; i < maxSize; i++)
            {
                foreach (var x in list)
                {
                    if (x.Timeline.Count - 1 >= i)
                        content += $"{x.Timeline[i].ToString()}\t";
                    else
                        content += ";";
                }
                content += "\n";
            }

            return Content(content);
        }
    }
}
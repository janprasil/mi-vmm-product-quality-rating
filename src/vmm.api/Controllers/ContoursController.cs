using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api")]
    public class ContoursController : Controller
    {
        private IContoursManager contoursManager;
        private readonly IHostingEnvironment appEnvironment;

        public ContoursController(IContoursManager contoursManager, IHostingEnvironment appEnvironment)
        {
            this.contoursManager = contoursManager;
            this.appEnvironment = appEnvironment;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Content("Create a POST Request with parameters 'files'");
        }

        [HttpPost]
        public ActionResult Post(IList<IFormFile> files)
        {
            var list = new List<Shape>();
            foreach (var file in files)
            {
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

            var maxSize = 0;
            foreach (var x in list)
            {
                if (x.Timeline.Count > maxSize) maxSize = x.Timeline.Count;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < maxSize; i++)
            {
                foreach (var x in list)
                {
                    if (x.Timeline.Count - 1 >= i)
                        sb.Append($"{x.Timeline[i].ToString()}\t");
                    else
                        sb.Append(";");
                }
                sb.Append("\n");
            }

            return Content(sb.ToString());
        }
    }
}
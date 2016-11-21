using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vmm.api.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Hosting;
using vmm.api.Models;

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

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            return Content("Create a POST Request with parametrs ´files´");
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post(IList<IFormFile> files)
        {
            //IEnumerable<Shape>
            String content = "";
            var list = new List<Shape>();
            foreach (var file in files)
            {
                var path = _appEnvironment.ContentRootPath;
                var filename = $@"{path}\upload\{file.FileName}";
                var contoursFilename = $"{file.FileName}.contures.jpeg";
                var target = $@"{path}\wwwroot\upload\{contoursFilename}";
                var urlTarget = $"{HttpContext.Request.Host}/upload/{contoursFilename}";

                using (FileStream fs = System.IO.File.Create(filename))
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

            for (int i = 0; i < maxSize; i++ )
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

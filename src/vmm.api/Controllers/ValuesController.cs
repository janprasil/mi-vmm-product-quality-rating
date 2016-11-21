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
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private IContoursManager _contoursManager;
        private readonly IHostingEnvironment _appEnvironment;

        public ValuesController(IContoursManager contoursManager, IHostingEnvironment appEnvironment)
        {
            _contoursManager = contoursManager;
            _appEnvironment = appEnvironment;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
           return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
                        content += $"{x.Timeline[i].ToString()};";
                    else
                        content += ";";
                }
                content += "\n";
            }

            return Content(content);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

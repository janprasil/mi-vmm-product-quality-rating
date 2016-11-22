using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/contours")]
    public class ContoursController : Controller
    {
        private IContoursManager contoursManager;
        private readonly IHostingEnvironment appEnvironment;
        private static List<Shape> list = new List<Shape>();

        public ContoursController(IContoursManager contoursManager, IHostingEnvironment appEnvironment)
        {
            this.contoursManager = contoursManager;
            this.appEnvironment = appEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(list);
        }

        [HttpGet]
        [Route("dtw")]
        public JsonResult GetDTW()
        {
            if (list.Count >= 2)
            {
                return Json(contoursManager.DynamicTimeWarping(list[0], list[1]));
            }
            return null;

        }

        [HttpPost]
        public JsonResult Post()
        {
            foreach (var file in Request.Form.Files)
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

            return Json(string.Empty);

            if (list.Count >= 2)
            {
                var dtw = contoursManager.DynamicTimeWarping(list[0], list[1]);
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

            return Json(sb.ToString());
        }


        [HttpDelete]
        public JsonResult Delete()
        {
            list.Clear();
            return Json(string.Empty);
        }
    }
}
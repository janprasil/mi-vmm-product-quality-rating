using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vmm.api.Data;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private IDbManager dbManager;
        private IContoursManager contoursManager;
        private readonly IHostingEnvironment appEnvironment;

        public ImagesController(IContoursManager contoursManager, IHostingEnvironment appEnvironment, IDbManager dbManager)
        {
            this.contoursManager = contoursManager;
            this.appEnvironment = appEnvironment;
            this.dbManager = dbManager;
        }

        [HttpGet]
        [Route("firebase")]
        public async Task<JsonResult> GetFirebase()
        {
            var result = await dbManager.PostAsync(new Shape()
            {
                Center = new Emgu.CV.Structure.MCvPoint2D64f(1.2, 3.4),
                ImageUrl = "http/test"
            });
            return Json(result);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(ShapeStorage.list);
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
                ShapeStorage.list.Add(result);
            }

            return Json(string.Empty);
        }

        [HttpDelete]
        public JsonResult Delete()
        {
            ShapeStorage.list.Clear();
            return Json(string.Empty);
        }
    }

    internal class FirebaseOptions
    {
        public Func<Task<string>> AuthTokenAsyncFactory { get; set; }
    }
}
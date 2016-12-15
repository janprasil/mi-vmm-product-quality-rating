using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/reference")]
    public class ReferenceController : Controller
    {
        private IDbManager dbManager;
        private IContoursManager contoursManager;
        private readonly IHostingEnvironment appEnvironment;
        private static List<Shape> list = new List<Shape>();

        public ReferenceController(IContoursManager contoursManager, IHostingEnvironment appEnvironment, IDbManager dbManager)
        {
            this.contoursManager = contoursManager;
            this.appEnvironment = appEnvironment;
            this.dbManager = dbManager;
        }

        [HttpGet]
        public async Task<JsonResult> Get(string id)
        {
            if (id == null)
            {
                var results = await dbManager.GetAllAsync<Shape>();
                return Json(results);
            }
            return Json(await dbManager.GetAsync<Shape>(new string[] { "ReferenceSamples", id }));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string id, double ct, double ctl)
        {
            var post = await dbManager.GetAsync<Shape>(id);
            var shape = post.Object;
            shape.CannyTreshold = ct;
            shape.CannyTreshodLinking = ctl;
            var result = contoursManager.Detect(shape.LocalPath, shape.ContourLocalPath, ct, ctl);

            await dbManager.PutAsync(id, result);

            return await Get(null);
        }

        [HttpPost]
        public async Task<JsonResult> Post()
        {
            foreach (var file in Request.Form.Files)
            {
                var path = appEnvironment.ContentRootPath;
                var filename = $@"{path}\wwwroot\uploads\references\originals\{file.FileName}";
                var target = $@"{path}\wwwroot\uploads\references\contours\{file.FileName}";
                var contourUrlTarget = $"/uploads/references/contours/{file.FileName}";
                var urlTarget = $"/uploads/references/originals/{file.FileName}";
                using (var fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var result = contoursManager.Detect(filename, target);

                result.ImageUrl = urlTarget;
                result.ContourImageUrl = contourUrlTarget;
                result.LocalPath = filename;
                result.ContourLocalPath = target;
                await dbManager.PostAsync(result);
            }

            return await Get(null);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string id)
        {
            await dbManager.DeleteAsync(new string[] { "ReferenceSamples", id });
            return await Get(null);
        }


        [HttpDelete]
        [Route("all")]
        public async Task<JsonResult> DeleteAll()
        {
            await dbManager.DeleteAllAsync<Shape>();
            return await Get(null);
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<JsonResult> Get(string sessionId, string id = null)
        {
            if (sessionId == null) return Json(await dbManager.GetAllAsync<List<Shape>>());
            else if (id == null) return Json(await dbManager.GetAllAsync<Shape>(new string[] { "Images", sessionId }));
            return Json(await dbManager.GetAsync<Shape>(new string[] { "Images", sessionId, id }));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string sessionId, string id, double ct, double ctl)
        {
            var shape = await dbManager.GetAsync<Shape>(new string[] { "Images", sessionId, id.ToString() });
            if (shape == null) return Json(new { id = "not_found", message = "Image or session was not found" });
            var result = contoursManager.Detect(shape.LocalPath, shape.ContourLocalPath, ct, ctl);

            shape.CannyTreshold = ct;
            shape.CannyTreshodLinking = ctl;
            shape.Center = result.Center;
            shape.Points = result.Points;
            shape.Timeline = result.Timeline;

            await dbManager.PutAsync(new string[] { "Images", sessionId, id.ToString() }, shape);

            return await Get(sessionId);
        }

        [HttpPost]
        public async Task<JsonResult> Post()
        {
            var list = new List<Shape>();
            foreach (var file in Request.Form.Files)
            {
                var path = appEnvironment.ContentRootPath;
                var filename = $@"{path}\wwwroot\uploads\originals\{file.FileName}";
                var target = $@"{path}\wwwroot\uploads\contours\{file.FileName}";
                var contourUrlTarget = $"/uploads/contours/{file.FileName}";
                var urlTarget = $"/uploads/originals/{file.FileName}";

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
                //ShapeStorage.list.Add(result);
                list.Add(result);
            }
            var session = await dbManager.PostAllAsync(list);
            return Json(session);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string sessionId, string id)
        {
            if (sessionId == null) return null;
            if (id == null)
            {
                await dbManager.DeleteAsync(new string[] { "Images", sessionId });
                return Json(new { id = "session_deleted", message = "Session was removed" });
            }
            await dbManager.DeleteAsync(new string[] { "Images", sessionId, id });
            return Json(new { id = "image_deleted", message = "Image was removed" });
        }
    }

    internal class FirebaseOptions
    {
        public Func<Task<string>> AuthTokenAsyncFactory { get; set; }
    }
}
﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
        private ILogger<ImagesController> log;

        public ReferenceController(IContoursManager contoursManager, IHostingEnvironment appEnvironment, IDbManager dbManager, ILogger<ImagesController> log)
        {
            this.contoursManager = contoursManager;
            this.appEnvironment = appEnvironment;
            this.dbManager = dbManager;
            this.log = log;
        }

        [HttpGet]
        public async Task<JsonResult> Get(string id = null)
        {
            if (id == null)
            {
                var results = await dbManager.GetAllAsync<Shape>();
                return Json(results);
            }
            var result = await dbManager.GetAsync<Shape>(new string[] { "ReferenceSamples", id });
            return Json(result);
        }

        [HttpPut]
        public async Task<JsonResult> Put(string id, double ct, double ctl)
        {
            var shape = await dbManager.GetAsync<Shape>(new string[] { "ReferenceSamples", id });
            if (shape == null) return Json(new { id = "not_found", message = "Image or session was not found" });

            if (!System.IO.File.Exists(shape.ContourLocalPath))
            {
                System.IO.File.Delete(shape.ContourLocalPath);
            }

            var sw = Stopwatch.StartNew();
            var result = contoursManager.Detect(shape.LocalPath, shape.ContourLocalPath, ct, ctl);
            sw.Stop();
            log.LogInformation($"PUT /reference {sw.ElapsedMilliseconds}");

            shape.CannyTreshold = ct;
            shape.CannyTreshodLinking = ctl;
            shape.Center = result.Center;

            shape.Points = result.Points;
            shape.Timeline = result.Timeline;

            await dbManager.PutAsync(new string[] { "ReferenceSamples", id }, shape);

            return await Get();
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

                var sw = Stopwatch.StartNew();
                var result = contoursManager.Detect(filename, target);
                sw.Stop();
                log.LogInformation($"POST /reference {file.FileName} {sw.ElapsedMilliseconds}");

                result.ImageUrl = urlTarget;
                result.ContourImageUrl = contourUrlTarget;
                result.LocalPath = filename;
                result.ContourLocalPath = target;
                await dbManager.PostAsync(result);
            }

            return await Get();
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string id)
        {
            await dbManager.DeleteAsync(new string[] { "ReferenceSamples", id });
            return await Get();
        }

        [HttpDelete]
        [Route("all")]
        public async Task<JsonResult> DeleteAll()
        {
            await dbManager.DeleteAllAsync<Shape>();
            return await Get();
        }
    }
}
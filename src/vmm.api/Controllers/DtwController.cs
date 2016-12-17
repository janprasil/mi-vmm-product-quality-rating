using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/dtw")]
    public class DtwController : Controller
    {
        private IContoursManager contoursManager;
        private IDbManager dbManager;
        private ILogger<ImagesController> log;

        public DtwController(IContoursManager contoursManager, IDbManager dbManager, ILogger<ImagesController> log)
        {
            this.contoursManager = contoursManager;
            this.dbManager = dbManager;
            this.log = log;
        }

        [HttpGet]
        public async Task<JsonResult> runDtw(string sessionId, string referenceId, int? turns)
        {
            var result = new List<FirebaseObject<Result>>();
            var images = await dbManager.GetAllAsync<Shape>(new string[] { "Images", sessionId });
            var reference = await dbManager.GetAsync<Shape>(new string[] { "ReferenceSamples", referenceId });

            if (images.Count == 0) return Json(new { id = "NO_IMAGES_SELECTED" });
            if (reference == null) return Json(new { id = "NO_REFERENCE_SELECTED" });
            foreach (var x in images)
            {
                var sw = Stopwatch.StartNew();
                var res = contoursManager.BestDTW(reference, x.Object, turns);
                sw.Stop();
                log.LogInformation($"GET /dtw {sw.ElapsedMilliseconds}");
                res.imageId = x.Key;
                res.referenceId = referenceId;
                result.Add(await dbManager.PostAsync(new string[] { "Results" }, res));
            }
            return Json(result);
        }
    }
}
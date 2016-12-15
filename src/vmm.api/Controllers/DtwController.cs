using Firebase.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using vmm.api.Data;
using vmm.api.Models;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/dtw")]
    public class DtwController : Controller
    {
        private IContoursManager contoursManager;
        private IDbManager dbManager;

        public DtwController(IContoursManager contoursManager, IDbManager dbManager)
        {
            this.contoursManager = contoursManager;
            this.dbManager = dbManager;
        }

        [HttpGet]
        public async Task<JsonResult> runDtw(string sessionId, string referenceId)
        {

            var result = new List<FirebaseObject<Result>>();
            var images = await dbManager.GetAllAsync<Shape>(new string[] { "Images", sessionId });
            var reference = await dbManager.GetAsync<Shape>(new string[] { "ReferenceSamples", referenceId });

            if (images.Count == 0) return Json(new { id = "NO_IMAGES_SELECTED" });
            if (reference == null) return Json(new { id = "NO_REFERENCE_SELECTED" });
            foreach (var x in images)
            {
                var r = new Result() {
                    result = contoursManager.DynamicTimeWarping(reference, x.Object),
                    imageId = x.Key,
                    referenceId = referenceId
                };
                result.Add(await dbManager.PostAsync(new string[] { "Results" }, r));
            }
            return Json(result);
        }
    }
}
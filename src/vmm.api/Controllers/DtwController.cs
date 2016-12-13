using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using vmm.api.Data;
using vmm.api.Services;

namespace vmm.api.Controllers
{
    [Route("api/dtw")]
    public class DtwController : Controller
    {
        private IContoursManager contoursManager;

        public DtwController(IContoursManager contoursManager)
        {
            this.contoursManager = contoursManager;
        }

        [HttpPost]
        public JsonResult runDtw(string refId)
        {
            if (ShapeStorage.list.Count >= 2)
            {
                return Json(contoursManager.DynamicTimeWarping(ShapeStorage.list[0], ShapeStorage.list[1]));
            }
            return Json(new { Id = "TWO_IMAGES_NEEDED", Message = "Upload at least two images" });
        }
    }
}
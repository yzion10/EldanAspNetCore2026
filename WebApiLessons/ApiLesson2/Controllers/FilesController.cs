using ApiLesson2.DataStores;
using ApiLesson2.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson2.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        [HttpGet("{name}")]
        public ActionResult GetFile(string name)
        {
            var safeName = Path.GetFileName(name);

            var baseDir = @"C:\Sources\GIT\CourseAspNetCore2026\Eldan";
            var path = Path.Combine(baseDir, safeName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var data = System.IO.File.ReadAllBytes(path);
            return File(data, "text/plain", name);
        }
    }
}

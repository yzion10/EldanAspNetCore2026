using ApiLesson3.DataStores;
using ApiLesson3.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ApiLesson3.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider _contentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider contentTypeProvider)
        {
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpGet("{name}")]
        public ActionResult GetFile(string name)
        {
            var safeName = Path.GetFileName(name);

            var baseDir = @"C:\Sources\GIT\CourseAspNetCore2026\Eldan";
            var path = Path.Combine(baseDir, safeName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var data = System.IO.File.ReadAllBytes(path);

            // מציאת סוג התוכן לפי סיומת הקובץ, אם לא נמצא נשתמש בסוג תוכן כללי
            _contentTypeProvider.TryGetContentType(path, out var contentType);
            return File(data, contentType ?? "application/octet-stream", name);
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file.Length > 1000000)
                return BadRequest("File is too big");

            var baseDir = @"C:\Sources\GIT\CourseAspNetCore2026\Eldan";
            var path = Path.Combine(baseDir, file.FileName);
            
            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            return NoContent();
        }
    }
}

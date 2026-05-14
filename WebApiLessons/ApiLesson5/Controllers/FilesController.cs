using ApiLesson5.DataStores;
using ApiLesson5.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ApiLesson5.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider _contentTypeProvider;
        private readonly ILogger<FilesController> _logger;

        public FilesController(FileExtensionContentTypeProvider contentTypeProvider, ILogger<FilesController> logger)
        {
            _contentTypeProvider = contentTypeProvider;
            _logger = logger;
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

            _logger.LogInformation($"Returning file {name} with content type {contentType ?? "application/octet-stream"}");

            return File(data, contentType ?? "application/octet-stream", name);
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file.Length > 1000000)
                return BadRequest("File is too big");

            var baseDir = @"C:\Sources\GIT\CourseAspNetCore2026\Eldan";
            var path = Path.Combine(baseDir, Guid.NewGuid().ToString() + $"_{file.FileName}");

            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            return NoContent();
        }
    }
}

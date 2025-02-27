using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Utils;

namespace API.Controllers
{
    public class FileController : ControllerBase
    {
        [HttpGet]
        [Route("files/{*path}")]
        public IActionResult GetFile(string path)
        {

            var fullPath = Path.Combine(FileUtils.BasePath, path);
            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var contentType = GetMimeTypeForFileExtension(fullPath);
            return File(fileStream, contentType);
        }


        private static string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string? contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }
}

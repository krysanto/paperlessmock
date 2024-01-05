using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Paperless.FileIO.Controllers
{
    [ApiController]
    [Route("/")]
    public class FilesApi : ControllerBase
    {
        private readonly string fileUploadDirectory;

        public FilesApi(IOptions<FileStorageServiceOptions> options)
        {
            fileUploadDirectory = options.Value.Path;
        }

        [HttpPost("{*filePath}"),
         Consumes(MediaTypeNames.Application.Octet)]
        public async Task<IActionResult> UploadFile(
            [FromRoute] string filePath,
            [FromBody, ModelBinder(typeof(StreamModelBinder)),
            SwaggerRequestBody("Content of the file", Required = true)] Stream body)
        {
            var path = GetSecureFullPath(fileUploadDirectory, filePath);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await body.CopyToAsync(fileStream);
                fileStream.Flush();
            }



            return Ok();
        }

        private string GetSecureFullPath(string fileUploadDirectory, string filePath)
        {
            filePath = filePath.TrimStart('/').TrimStart('\\');

            string path = Path.GetFullPath(Path.Combine(fileUploadDirectory, filePath));

            if (!path.StartsWith(fileUploadDirectory))
            {
                throw new InvalidOperationException("Not a valid path");
            }
            else
            {
                return path;
            }
        }

        [HttpGet("{*filePath}")]
        public IActionResult DownloadFile([FromRoute] string filePath)
        {
            var path = GetSecureFullPath(fileUploadDirectory, filePath);

            if (System.IO.File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var contentType = "application/octet-stream";

                return File(fileStream, contentType, path);
            }

            return NotFound();
        }


        [HttpDelete("{*filePath}")]
        public IActionResult DeleteFile([FromRoute] string filePath)
        {
            var path = GetSecureFullPath(fileUploadDirectory, filePath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                return Ok();
            }

            return NotFound();
        }

        [HttpPut("{*filePath}")]
        public IActionResult RenameFile([FromRoute] string filePath, [ModelBinder(typeof(TextPlainModelBinder)), FromBody] string newFilePath)
        {
            var oldPath = GetSecureFullPath(fileUploadDirectory, filePath);

            var newPath = GetSecureFullPath(fileUploadDirectory, newFilePath);

            if (System.IO.File.Exists(oldPath) && !System.IO.File.Exists(newPath))
            {
                System.IO.File.Move(oldPath, newPath);
                return Ok();
            }

            return NotFound();
        }

    }
}

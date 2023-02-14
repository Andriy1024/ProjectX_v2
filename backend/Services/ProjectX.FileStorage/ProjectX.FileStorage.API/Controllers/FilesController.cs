using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.FileStorage.Infrastructure.Handlers;
using System.Reflection;

namespace ProjectX.FileStorage.API.Controllers
{
    [Route("api/files")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FilesController : ProjectXController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("host")]
        public async Task<IActionResult> GetHost()
        {
            return Ok(new {
                HttpHost = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}",
                AssemblyRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                WWWROOT = _webHostEnvironment.WebRootPath
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilesAsync(CancellationToken cancellationToken)
        {
            return MapResponse(await Mediator.Send(new FilesQuery(), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync([FromForm] UploadFileCommand command, CancellationToken cancellation = default) 
        {
            return MapResponse(await Mediator.Send(command, cancellation));
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadAsync([FromRoute] Guid id, CancellationToken cancellation = default)
        {
            var response = await Mediator.Send(new DownloadFileQuery() { Id = id }, cancellation);

            if (response.IsFailed) 
            {
                return BadRequest(response);
            }

            return File(response.Data!.File, response.Data.MimeType);
        }
    }
}

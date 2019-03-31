using DataManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        FilesService _filesService;
        /// <summary>
        /// Injecting Dependencies
        /// </summary>
        /// <param name="filesService"></param>
        public FilesController(FilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("[action]")]
        public void PostFile()
        {
            _filesService.SaveFile(Request.Body);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        FilesService _filesService;
        public FilesController(FilesService filesService)
        {
            _filesService = filesService;
        }
        [HttpPost("[action]"), DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _filesService.UploadFile(file, "user_name", "password");
            }
            return null;
        }
    }
}
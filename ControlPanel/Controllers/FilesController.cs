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
        public async Task<IActionResult> UploadFile()
        {
            //TODO Check if the form or files is null
            var file = Request.Form.Files[0];
            //TODO Check username password avaialble 
            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            if (file.Length > 0)
            {
                await _filesService.UploadFile(file, dict["username"], dict["password"]);
            }
            return new OkResult();
        }
    }
}
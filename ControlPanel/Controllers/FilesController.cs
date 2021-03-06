﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public async Task<HttpResponseMessage> UploadFile()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0 && file.FileName.EndsWith(".zip"))
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
               // Uploading file to datamanager
                return await _filesService.UploadFile(file, dict["username"], dict["password"]);
            }
            return new HttpResponseMessage(HttpStatusCode.Forbidden) { ReasonPhrase = "Cannot detect a valid zip file" };
        }
    }
}
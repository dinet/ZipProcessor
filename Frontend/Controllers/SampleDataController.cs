using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Frontend.Helpers;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        [HttpPost("[action]"), DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                #region Creating Json  
                ZipArchive t = new ZipArchive(file.OpenReadStream());
                Node root = new Node() { Title ="."};
                foreach (var item in t.Entries)
                {
                    var split = item.FullName.Split('/');
                    Node parent = root;
                    Node child = null;
                    foreach (var folder in split)
                    {
                        if (!String.IsNullOrEmpty(folder))
                        {
                            child = parent.Contents.Find(n => n.Title == folder);
                            if (child == null)
                            {
                                child = new Node { Title = folder };
                                parent.Contents.Add(child);
                            }
                            parent = child;
                        }
                    }
                }
                var json = JsonConvert.SerializeObject(root);
                #endregion

                #region Encryption
                EncryptionHelper ec = new EncryptionHelper(null, null);
                ec.EncryptAesManaged(json);

 
                #endregion

            }
            return Json("Upload Successful.");
        }
        class Node
        {
            public Node()
            {
                Contents = new List<Node>();
            }

            public string Title { get; set; }
            public List<Node> Contents { get; set; }
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}

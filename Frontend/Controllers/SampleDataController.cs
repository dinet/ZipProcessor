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
using System.Net.Http;
using System.Text;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        HttpClient client = new HttpClient();
        EncryptionHelper _encryptionHelper; 

        public SampleDataController(EncryptionHelper encryptionHelper)
        {
            // Should get from config
            client.BaseAddress = new Uri("https://localhost:44306/");
            _encryptionHelper = encryptionHelper;
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
                Node root = new Node() { Title = "." };
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
                byte[] encrypted = _encryptionHelper.Encrypt(json);                
                string decrtypted = _encryptionHelper.Decrypt(encrypted);
                #endregion

                #region Send Request
                string userName = "user_name";
                string password = "password";
                byte[] usernamePassword = _encryptionHelper.Encrypt($"{userName}:{password}");
                string userNamePasswordString = Convert.ToBase64String(usernamePassword);
                string encryptedJsonString = Convert.ToBase64String(encrypted);
                this.client.DefaultRequestHeaders.Add("Authorization", $"Basic {userNamePasswordString}");
                this.client.PostAsync("api/values/PostJson", new StringContent(encryptedJsonString));
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
    }
}

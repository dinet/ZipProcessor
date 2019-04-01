using ControlPanel.Models;
using Frontend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class FilesService
    {
        HttpClient _client = new HttpClient();
        readonly EncryptionHelper _encryptionHelper;
        readonly IConfiguration _config;

        public FilesService(EncryptionHelper encryptionHelper, IConfiguration config)
        {
            //Setting the base address of the data manager
            _client.BaseAddress = new Uri(config.GetValue<string>("DataManagerApi:BaseUrl"));
            _config = config;
            _encryptionHelper = encryptionHelper;
        }
        public void UploadFile(IFormFile zipFile, string username, string password)
        {
            //Encrypt username and password
            byte[] usernamePassword = _encryptionHelper.Encrypt($"{username}:{password}");
            // Convert encrypted password(byte array) to base 64 string
            string userNamePasswordString = Convert.ToBase64String(usernamePassword);
            string jsonString = this.ConvertFileToJSONString(zipFile);
            byte[] encrypted = _encryptionHelper.Encrypt(jsonString);
            string encryptedJsonString = Convert.ToBase64String(encrypted);
            this._client.DefaultRequestHeaders.Add("Authorization", $"Basic {userNamePasswordString}");
            this._client.PostAsync(_config.GetValue<string>("DataManagerApi:UploadFilePath"), new StringContent(encryptedJsonString));
        }

        private string ConvertFileToJSONString(IFormFile zipFile)
        {
            ZipArchive archive = new ZipArchive(zipFile.OpenReadStream());
            // Retriving the file name
            string fileName = ContentDispositionHeaderValue.Parse(zipFile.ContentDisposition).FileName.Trim('"');
            Folder root = new Folder() { Title = fileName };
            foreach (ZipArchiveEntry item in archive.Entries)
            {
                string[] split = item.FullName.Split('/');
                Folder parent = root;
                Folder child = null;
                foreach (string folder in split)
                {
                    if (!string.IsNullOrEmpty(folder))
                    {
                        child = parent.Contents.Find(n => n.Title == folder);
                        if (child == null)
                        {
                            child = new Folder { Title = folder };
                            parent.Contents.Add(child);
                        }
                        parent = child;
                    }
                }
            }
            string jsonString = JsonConvert.SerializeObject(root);
            return jsonString;
        }

    }
}


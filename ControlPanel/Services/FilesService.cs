using ControlPanel.Models;
using Frontend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class FilesService
    {
        readonly EncryptionHelper _encryptionHelper;
        readonly IConfiguration _config;

        public FilesService(EncryptionHelper encryptionHelper, IConfiguration config)
        {
            _config = config;
            _encryptionHelper = encryptionHelper;
        }
        public async Task<HttpResponseMessage> UploadFile(IFormFile zipFile, string username, string password)
        {
            try
            {
                //Encrypt username and password
                byte[] usernamePassword = _encryptionHelper.Encrypt($"{username}:{password}");
                // Convert encrypted password(byte array) to base 64 string
                string userNamePasswordString = Convert.ToBase64String(usernamePassword);
                //Converting zipfile to Json string
                string jsonString = this.ConvertFileToJSONString(zipFile);
                //Encrypting the json string
                byte[] encrypted = _encryptionHelper.Encrypt(jsonString);
                // Convert encrypted json string(byte array) to base 64 string
                string encryptedJsonString = Convert.ToBase64String(encrypted);

                HttpClient client = new HttpClient();
                //Setting the base address of the data manager from config
                client.BaseAddress = new Uri(_config.GetValue<string>("DataManagerApi:BaseUrl"));
                //Setting up the authorization header
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {userNamePasswordString}");
                //sending data to datamanager
                return await client.PostAsync(_config.GetValue<string>("DataManagerApi:UploadFilePath"), new StringContent(encryptedJsonString));
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
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


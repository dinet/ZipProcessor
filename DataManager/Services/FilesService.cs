using DataManager.DAL;
using DataManager.Helpers;
using DataManager.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager.Services
{
    public class FilesService
    {
        EncryptionHelper _encryptionHelper;
        DatabaseService _databaseService;

        public FilesService(EncryptionHelper encryptionHelper, DatabaseService databaseService)
        {
            _encryptionHelper = encryptionHelper;
            _databaseService = databaseService;
        }
        public void SaveFile(Stream fileStream)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                //Converting plain text to byte array 
                string plainText = reader.ReadToEnd();
                byte[] array = Convert.FromBase64String(plainText);

                //Decrypting the json file 
                string json = _encryptionHelper.Decrypt(array);
                Folder item = JsonConvert.DeserializeObject<Folder>(json);

                // Saving to the database
                _databaseService.Create(item);
            }
        }

    }
}

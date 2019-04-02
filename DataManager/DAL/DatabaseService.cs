using DataManager.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.DAL
{
    public class DatabaseService
    {
        private readonly IMongoCollection<Folder> _records;

        public DatabaseService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("FileStoreDb"));
            var database = client.GetDatabase("FileStoreDb");
            _records = database.GetCollection<Folder>("Folders");
        }

        public Folder Create(Folder folder)
        {
            _records.InsertOne(folder);
            return folder;
        }
    }
}

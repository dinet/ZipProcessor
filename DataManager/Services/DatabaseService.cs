using DataManager.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.Services
{
    public class DatabaseService
    {
        private readonly IMongoCollection<Record> _records;

        public DatabaseService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("FileStoreDb"));
            var database = client.GetDatabase("FileStoreDb");
            _records = database.GetCollection<Record>("Files");
        }

        public List<Record> Get()
        {
            return _records.Find(book => true).ToList();
        }

        //public Record Get(string id)
        //{
        //    return _records.Find<Record>(book => book.Id == id).FirstOrDefault();
        //}

        public Record Create(Record book)
        {
            _records.InsertOne(book);
            return book;
        }
    }
}

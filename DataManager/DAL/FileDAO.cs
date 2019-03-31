using DataManager.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataManager.DAL
{
    public class FileDAO
    {
        [BsonElement("UserId")]
        public int UserId { get; set; }

        [BsonElement("Node")]
        public Folder Node { get; set; }
    }
}

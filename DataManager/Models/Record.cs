using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataManager.Models
{
    public class Record
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }
        
        [BsonElement("UserId")]
        public int UserId { get; set; }

        [BsonElement("Node")]
        public Node Node { get; set; }
    }
}

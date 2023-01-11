using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaterTap.Api.Data
{
    public class UserInfo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

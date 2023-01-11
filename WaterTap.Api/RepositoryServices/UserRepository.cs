using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WaterTap.Api.Data;
using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserInfo> _userInfoCollecction;
        public UserRepository(IOptions<WaterTapDatabase> watertapDatabase)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var mongoClient = new MongoClient("mongodb://ramprakash:Ramprakash%202001@watertap.uwy78ec.mongodb.net/watertapdb");
            var mongoDb = mongoClient.GetDatabase("watertapdp");

            _userInfoCollecction = mongoDb.GetCollection<UserInfo>("watertapinfo");
        }

        public List<UserModel> GetAllUsers()
        {
            var records = _userInfoCollecction.AsQueryable().Select(x =>new UserModel()
            {
                UserName = x.UserName,
                EmailId = x.EmailId,
                Password = x.Password,
            }).ToList();

            return records;
        }

        public UserModel GetUser(string emailId)
        {
            var user = _userInfoCollecction.AsQueryable().Where(x => x.EmailId.ToLower() == emailId.ToLower())
                .Select(x => new UserModel()
            {
                UserName = x.UserName,
                EmailId = x.EmailId,
                Password = x.Password,
            }).FirstOrDefault();

            if(user == null)
            {
                return null;
            }

            return user;

        }

        public string AddNewUser(UserModel user)
        {
            var userInfo = new UserInfo()
            {
                UserName = user.UserName,
                EmailId = user.EmailId,
                Password = user.Password,
            };

            _userInfoCollecction.InsertOne(userInfo);

            return user.EmailId;
        }

        public async Task<string> UpdateUser(string emailId, UserModel user)
        {
            var userDetailFilter = Builders<UserInfo>.Filter.Eq(x => x.EmailId, emailId);
            var updateUserDetail = Builders<UserInfo>.Update
                .Set(x => x.EmailId, emailId)
                .Set(x => x.UserName, user.UserName)
                .Set(x => x.Password, user.Password);

            await _userInfoCollecction.UpdateOneAsync(userDetailFilter, updateUserDetail);
            return user.EmailId;
        }
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WaterTap.Api.Data;
using WaterTap.Api.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace WaterTap.Api.RepositoryServices
{
    public class TapDetailRepository : ITapDetailRepository
    {
        private readonly IMongoCollection<TapInfo> _tapInfoCollecction;
        public TapDetailRepository(IOptions<WaterTapDatabase> watertapDatabase)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var mongoClient = new MongoClient("mongodb://ramprakash:Ramprakash%202001@watertap.uwy78ec.mongodb.net/watertapdb");
            var mongoDb = mongoClient.GetDatabase("watertapdp");

            _tapInfoCollecction = mongoDb.GetCollection<TapInfo>("tapinfo");
        }

        public List<TapDetailModel> GetAllTapDetails()
        {
            var tapdetails = _tapInfoCollecction.AsQueryable().Select(x => new TapDetailModel()
            {
                TapId = x.Id,
                Name = x.Name,
                Type = x.Type,
                Amount = x.Amount
            }).ToList();

            return tapdetails;
        }

        public TapDetailModel? GetTapDetail(int id)
        {
            return _tapInfoCollecction.AsQueryable().Where(x => x.Id == id)
                .Select(x => new TapDetailModel()
                {
                    TapId = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Amount = x.Amount
                }).FirstOrDefault();
        }

        public int AddNewTapDetail(TapDetailModel tap)
        {
            var ids = _tapInfoCollecction.AsQueryable().Select(x => x.Id).ToList();
            var id = ids.Any() ? ids.Max(x => x) + 1 : 1;

            var tapDetail = new TapInfo()
            {
                Id = id,
                Name = tap.Name,
                Type = tap.Type,
                Amount = tap.Amount,
            };

            _tapInfoCollecction.InsertOne(tapDetail);

            return tapDetail.Id;
        }

        public async Task<int> UpdateTapDetail(int id, TapDetailModel incomingTapDetail)
        {
            var tapDetailFilter = Builders<TapInfo>.Filter.Eq(x => x.Id, id);
            var updateTapDetail = Builders<TapInfo>.Update
                .Set(x => x.Name, incomingTapDetail.Name)
                .Set(x => x.Type, incomingTapDetail.Type)
                .Set(x => x.Amount, incomingTapDetail.Amount);

            await _tapInfoCollecction.UpdateOneAsync(tapDetailFilter, updateTapDetail);
            return id;
        }

        public async Task<int> DeleteTapDetail(int id)
        {
            await _tapInfoCollecction.DeleteOneAsync(x => x.Id == id);
            return id;
        }
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WaterTap.Api.Data;
using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public class TapEntryRepository : ITapEntryRepository
    {
        private readonly IMongoCollection<TapEntry> _tapEntryCollecction;
        private readonly IMongoCollection<TapInfo> _tapInfoCollection;
        public TapEntryRepository(IOptions<WaterTapDatabase> watertapDatabase)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var mongoClient = new MongoClient("mongodb://ramprakash:Ramprakash%202001@watertap.uwy78ec.mongodb.net/watertapdb");
            var mongoDb = mongoClient.GetDatabase("watertapdp");

            _tapEntryCollecction = mongoDb.GetCollection<TapEntry>("tap-entries");
            _tapInfoCollection = mongoDb.GetCollection<TapInfo>("tapinfo");
        }

        public List<TapEntryDetailModel> GetAllTapEntryDetails()
        {
            var tapEntryDetails = _tapEntryCollecction.AsQueryable().OrderByDescending(x => x.ProcessedAt)
                .Select(x => new TapEntryDetailModel()
                {
                    Id = x.Id,
                    TapId = x.TapId,
                    TapName = x.TapName,
                    Date = x.Date,
                    Quantity = x.Quantity,
                    Amount = x.Amount
                }).ToList();

            return tapEntryDetails;
        }

        public TapEntryDetailModel? GetTapEntryDetail(int id)
        {
            return _tapEntryCollecction.AsQueryable().Where(x => x.Id == id)
                .Select(x => new TapEntryDetailModel()
                {
                    Id = x.Id,
                    TapId = x.TapId,
                    TapName = x.TapName,
                    Date = x.Date,
                    Quantity = x.Quantity,
                    Amount = x.Amount
                }).FirstOrDefault();
        }

        public int AddNewTapEntryDetail(TapEntryDetailModel incomingTapEntryDetail)
        {
            var tapDetail = _tapInfoCollection.AsQueryable().FirstOrDefault(x => x.Id == incomingTapEntryDetail.TapId);

            if(tapDetail == null)
            {
                throw new Exception($"Tap information not found with tap id : {incomingTapEntryDetail.TapId}");
            }

            var ids = _tapEntryCollecction.AsQueryable().Select(x => x.Id);

            var id = ids.Any() ? ids.Max() + 1 : 1;

            var tapEntryDetail = new TapEntry()
            {
                Id = id,
                TapId = incomingTapEntryDetail.TapId,
                TapName = tapDetail.Name,
                Date = incomingTapEntryDetail.Date,
                Quantity = incomingTapEntryDetail.Quantity,
                Amount = tapDetail.Amount * incomingTapEntryDetail.Quantity,
                ProcessedAt = DateTime.UtcNow
            };

            _tapEntryCollecction.InsertOne(tapEntryDetail);

            return tapEntryDetail.Id;
        }

        public async Task<int> UpdateTapEntryDetail(int id, TapEntryDetailModel incomingTapEntryDetail)
        {
            var tapDetail = _tapInfoCollection.AsQueryable().FirstOrDefault(x => x.Id == incomingTapEntryDetail.TapId);

            if (tapDetail == null)
            {
                throw new Exception($"Tap information not found with tap id : {incomingTapEntryDetail.TapId}");
            }

            var tapDetailFilter = Builders<TapEntry>.Filter.Eq(x => x.Id, id);
            var updateTapDetail = Builders<TapEntry>.Update
                .Set(x => x.TapName, incomingTapEntryDetail.TapName)
                .Set(x => x.TapId, incomingTapEntryDetail.TapId)
                .Set(x => x.Quantity, incomingTapEntryDetail.Quantity)
                .Set(x => x.Date, incomingTapEntryDetail.Date)
                .Set(x => x.Amount, tapDetail.Amount * incomingTapEntryDetail.Quantity)
                .Set(x => x.ProcessedAt, DateTime.UtcNow);

            await _tapEntryCollecction.UpdateOneAsync(tapDetailFilter, updateTapDetail);
            return id;
        }

        public async Task<int> DeleteTapEntryDetail(int id)
        {
            await _tapEntryCollecction.DeleteOneAsync(x => x.Id == id);
            return id;
        }
    }
}

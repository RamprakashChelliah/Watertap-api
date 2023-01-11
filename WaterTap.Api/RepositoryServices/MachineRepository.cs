using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ParkBee.MongoDb;
using System.Globalization;
using WaterTap.Api.Data;
using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public class MachineRepository : IMachineRepository
    {
        private readonly IMongoCollection<MachineDetail> _machineDetailCollecction;

        public MachineRepository(IOptions<WaterTapDatabase> watertapDatabase)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var mongoClient = new MongoClient("mongodb://ramprakash:Ramprakash%202001@watertap.uwy78ec.mongodb.net/watertapdb");
            var mongoDb = mongoClient.GetDatabase("watertapdp");

            _machineDetailCollecction = mongoDb.GetCollection<MachineDetail>("machines");

        }

        public List<MachineModel> GetAllMachines()
        {
            var machineDetails = _machineDetailCollecction.AsQueryable().Select(x => new MachineModel()
            {
                MachineId = x.Id,
                Name = x.Name,
            }).ToList();

            return machineDetails;
        }

        public List<MachineDetailModel> GetMachineDetail(Guid machineId, int month, int year)
        {
            var machineDetailList = new List<MachineDetailModel>();
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, daysInMonth);

            var machineDetail = _machineDetailCollecction.AsQueryable().FirstOrDefault(x => x.Id == machineId);

            if(machineDetail == null)
            {
                return new List<MachineDetailModel>();
            }

            var monthRecords = _machineDetailCollecction.AsQueryable().Where(x => x.Id == machineId &&
            x.MachineEntries != null).SelectMany(x => x.MachineEntries)
            .Where(x => x.Date >= startDate && x.Date <= endDate).ToList();

            for(int i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(year, month, i);
                var isExistRecord = monthRecords.Find(x => x.Date == date);

                if(isExistRecord == null)
                {
                    machineDetailList.Add(new MachineDetailModel
                    {
                        MachineId = machineDetail.Id,
                        Name = machineDetail.Name,
                        Date = date
                    });

                    continue;
                }

                var currentlyRunningRecord = isExistRecord.PlayHistories.Where(x => x.EndTime == null).FirstOrDefault();

                if (currentlyRunningRecord == null)
                {
                    machineDetailList.Add(new MachineDetailModel
                    {
                        MachineId = machineDetail.Id,
                        Name = machineDetail.Name,
                        Date = date,
                        TimeDuration = isExistRecord.TimeDuration
                    });
                    continue;
                }
                else
                {
                    var timeDuration = (DateTime.UtcNow - currentlyRunningRecord.StartTime).TotalSeconds;
                    machineDetailList.Add(new MachineDetailModel
                    {
                        MachineId = machineDetail.Id,
                        Name = machineDetail.Name,
                        Date = date,
                        TimeDuration = timeDuration
                    });
                }
            }

            return machineDetailList;
                
        }

        public Guid AddNewMachineDeatil(MachineModel incomingMachine)
        {
            var machineDetail = new MachineDetail()
            {
                Id = Guid.NewGuid(),
                Name = incomingMachine.Name,
                InsertedDate = DateTime.UtcNow
            };

            _machineDetailCollecction.InsertOne(machineDetail);

            return machineDetail.Id;
        }

        public async Task<Guid> UpdateMachineDetail(Guid id, MachineModel incomingMachineDetail)
        {
            var machineDetailFilter = Builders<MachineDetail>.Filter.Eq(x => x.Id, id);
            var updatedMachineDetail = Builders<MachineDetail>.Update
                .Set(x => x.Name, incomingMachineDetail.Name);

            await _machineDetailCollecction.UpdateOneAsync(machineDetailFilter, updatedMachineDetail);
            return id;
        }


        public double ManageMachineTimeDuration(Guid machineId, string incomingDate, bool isStarted)
        {
            var date = DateTime.ParseExact(incomingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var startDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var machineDetail = _machineDetailCollecction.AsQueryable().FirstOrDefault(x => x.Id == machineId);

            if(machineDetail == null)
            {
                throw new Exception("Machine detail not found");
            }

            var dayRecord = machineDetail.MachineEntries.Find(x => x.Date == startDate);

            if(dayRecord == null && !isStarted)
            {
                throw new Exception($"Past play record not found with date: {startDate} ");
            }

            if(dayRecord == null)
            {
                var playHistories = new List<PlayHistory>();

                playHistories.Add(new PlayHistory
                {
                    StartTime = DateTime.UtcNow
                });

                machineDetail.MachineEntries.Add(new MachineEntry
                {
                    Date = startDate,
                    PlayHistories = playHistories
                });
            }
            else if(dayRecord != null)
            {
                if (isStarted)
                {
                    var hasAlreadyRunning = dayRecord.PlayHistories.Any(x => x.EndTime == null);

                    if (hasAlreadyRunning)
                    {
                        throw new Exception("Machine was already running. So you can only stop");
                    }

                    var playHistories = new List<PlayHistory>();

                    playHistories.Add(new PlayHistory
                    {
                        StartTime = DateTime.UtcNow
                    });

                    machineDetail.MachineEntries.Add(new MachineEntry
                    {
                        Date = startDate,
                        PlayHistories = playHistories
                    });
                }
                else
                {
                    var alreadyRunning = dayRecord.PlayHistories.FirstOrDefault(x => x.EndTime == null);

                    if (alreadyRunning == null)
                    {
                        throw new Exception("Machine was not running. So you cannot stop");
                    }

                    alreadyRunning.EndTime = DateTime.UtcNow;

                    dayRecord.TimeDuration = (alreadyRunning.EndTime.Value - alreadyRunning.StartTime).TotalSeconds;
                }
            }

            var machineDetailFilter = Builders<MachineDetail>.Filter.Eq(x => x.Id, machineId);
            var updatedMachineDetail = Builders<MachineDetail>.Update
                .Set(x => x.MachineEntries.Find(x => x.Date == startDate), dayRecord);

            //_machineDetailCollecction.UpdateOne<MachineDetail>();
            return dayRecord.TimeDuration;

        }

        public async Task<Guid> DeleteMachineDetail(Guid id)
        {

            var machineDeatil = _machineDetailCollecction.AsQueryable().FirstOrDefault(x => x.Id == id);

            if(machineDeatil == null)
            {
                throw new Exception("Machine details not found");
            }

            if (machineDeatil.MachineEntries.Any())
            {
                throw new Exception("Machine has some entries. So, you cannot delete the machine");
            }

            await _machineDetailCollecction.DeleteOneAsync(x => x.Id == id);
            return id;
        }
    }
}

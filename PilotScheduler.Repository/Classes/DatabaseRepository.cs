using System;
using System.IO;
using System.Linq;
using LiteDB;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Repository.Models;
using PilotScheduler.Service.Models.Classes;
using PilotScheduler.Service.Models.Interfaces;

namespace PilotScheduler.Repository.Classes
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IDataConfiguration _pilotData;
        private readonly IPilotAvailabilityChecker _pilotAvailabilityChecker;
        
        public DatabaseRepository(IDataConfiguration pilotData, IPilotAvailabilityChecker pilotAvailabilityChecker)
        {
            _pilotData = pilotData;
            _pilotAvailabilityChecker = pilotAvailabilityChecker;
        }

        /// <summary>
        /// This method will check if there is a request record for the available pilot
        /// it tries to select an availble pilot without a flight request record, which is what we want
        /// if the available pilots all have flights request
        /// it picks the one with lowest flight request record and returns the id of the pilot with some extra metadata.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="departureDate"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>
        public DummyPilotRequestRecord RequestPilot(string location, DateTime departureDate, DateTime returnDate)
        {
            var availablePilots =
                _pilotAvailabilityChecker.GetAvailablePilots(location, departureDate, returnDate, _pilotData.Crew);
            
            DummyPilotRequestRecord requestedPilotRecord = null;
            var dbDirectory = Path.Combine(Environment.CurrentDirectory, "tempdata.db");
            
            using var db = new LiteDatabase($"Filename={dbDirectory};Mode=Exclusive");
            
            var pilotRequestCollection = db.GetCollection<DummyPilotRequestRecord>("DummyPilotRequestRecord");
            for (int i = 0; i < availablePilots.Length; i++)
            {
                var isPilotScheduled = pilotRequestCollection.Exists(x => x.PilotId.Equals(availablePilots[i].Id));
                if (!isPilotScheduled)
                {
                    requestedPilotRecord = new DummyPilotRequestRecord()
                    {
                        Id = Guid.NewGuid(),
                        PilotId = availablePilots[i].Id,
                        RequestCount = 1
                    };
                    pilotRequestCollection.Insert(requestedPilotRecord);
                    return requestedPilotRecord;
                }
            }
            
            var availablePilotIds = availablePilots.Select(x => x.Id);
            var bsonIds = new BsonArray();
            foreach (var pilotId in availablePilotIds)
            {
                bsonIds.Add(pilotId);
            }

            requestedPilotRecord = pilotRequestCollection.Find(Query.In("PilotId", bsonIds))
                .OrderBy(x => x.RequestCount).FirstOrDefault();
            requestedPilotRecord.RequestCount += 1;
            pilotRequestCollection.Update(requestedPilotRecord);
            
            return requestedPilotRecord;
        }

        public Crew ScheduleFlight(int pilotId, DateTime departureDate, DateTime returnDate)
        {
            var retrievedPilot = _pilotData.Crew.FirstOrDefault(x => x.Id.Equals(pilotId));
            if (null == retrievedPilot)
                return null; 
            
            using var db = new LiteDatabase("tempdata.db");
            var pilotRequestCollection = db.GetCollection<DummyPilotScheduleRecord>("DummyPilotScheduleRecord");
            
            var scheduleDocument = new DummyPilotScheduleRecord()
            {
                PilotId = retrievedPilot.Id,
                DepartureDate = departureDate,
                ReturnDate = returnDate
            };
            pilotRequestCollection.Insert(scheduleDocument);
            return retrievedPilot;
        }
    }
}
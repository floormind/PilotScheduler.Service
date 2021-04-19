using System;
using LiteDB;

namespace PilotScheduler.Repository.Models
{
    public class DummyPilotScheduleRecord
    {
        [BsonId]
        public Guid Id { get; set; }
        public int PilotId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        
        public int RequestCount { get; set; }
    }
}
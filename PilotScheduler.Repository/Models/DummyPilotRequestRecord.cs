using System;
using LiteDB;

namespace PilotScheduler.Repository.Models
{
    public class DummyPilotRequestRecord
    {
        [BsonId]
        public Guid Id { get; set; }
        public int PilotId { get; set; }
        public int RequestCount { get; set; }
    }
}
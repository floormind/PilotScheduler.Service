using System;
using PilotScheduler.Repository.Models;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Interfaces
{
    public interface IDatabaseRepository
    {
        public DummyPilotRequestRecord RequestPilot(string location, DateTime departureDate, DateTime returnDate);

        public Crew ScheduleFlight(int pilotId, DateTime departureDate, DateTime returnDate);
    }
}
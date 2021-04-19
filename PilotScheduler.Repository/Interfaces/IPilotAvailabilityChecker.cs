using System;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Interfaces
{
    public interface IPilotAvailabilityChecker
    {
        public Crew[] GetAvailablePilots(string location, DateTime departureDate, DateTime returnDate, Crew[] crews);
    }
}
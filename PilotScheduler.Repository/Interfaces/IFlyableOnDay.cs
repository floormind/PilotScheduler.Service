using System;
using System.Collections.Generic;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Interfaces
{
    public interface IFlyableOnDay
    {
        public IList<Crew> CanFlyAtSchedule(DateTime departureDate, DateTime returnDate, Crew[] crews);
    }
}
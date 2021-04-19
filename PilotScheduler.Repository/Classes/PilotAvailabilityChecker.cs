using System;
using System.Linq;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Classes
{
    public class PilotAvailabilityChecker : IPilotAvailabilityChecker
    {
        private readonly IFlyableFromLocation _flyableFromLocation;
        private readonly IFlyableOnDay _flyableOnDay;

        public PilotAvailabilityChecker(IFlyableFromLocation flyableFromLocation, IFlyableOnDay flyableOnDay)
        {
            _flyableFromLocation = flyableFromLocation;
            _flyableOnDay = flyableOnDay;
        }
        
        public Crew[] GetAvailablePilots(string location, DateTime departureDate, DateTime returnDate, Crew[] crews)
        {
            var pilotsWithBaseOfSpecifiedLocation = _flyableFromLocation.CanFlyFromLocation(location, crews);
            if (pilotsWithBaseOfSpecifiedLocation.Count == 0)
                return null;
            
            var pilotsAvailableToWorkOnSpecifiedDate =
                _flyableOnDay.CanFlyAtSchedule(departureDate, returnDate, pilotsWithBaseOfSpecifiedLocation.ToArray());
            if (pilotsAvailableToWorkOnSpecifiedDate.Count == 0)
                return null;
            
            return pilotsAvailableToWorkOnSpecifiedDate.ToArray();
        }
    }
}
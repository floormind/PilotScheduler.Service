using System.Collections.Generic;
using System.Linq;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Classes
{
    public class FlyableFromLocation : IFlyableFromLocation
    {
        public IList<Crew> CanFlyFromLocation(string location, Crew[] crews)
        {
            return crews.Where(pilot => pilot.Base == location).ToList();
        }
    }
}
using System.Collections.Generic;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Interfaces
{
    public interface IFlyableFromLocation
    {
        public IList<Crew> CanFlyFromLocation(string location, Crew[] crews);
    }
}
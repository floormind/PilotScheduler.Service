using PilotScheduler.Service.Models.Classes;
using PilotScheduler.Service.Models.Interfaces;

namespace PilotScheduler.Repository.Classes
{
    public class DataConfiguration : IDataConfiguration
    {
        public Crew[] Crew { get; set; }
    }
}
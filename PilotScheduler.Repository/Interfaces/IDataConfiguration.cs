using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Service.Models.Interfaces
{
    public interface IDataConfiguration
    {
        public Crew[] Crew { get; set; }
    }
}
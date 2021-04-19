using System;

namespace PilotScheduler.Service.Models.Classes
{
    public class ScheduleRequestModel
    {
        public int PilotId { get; set; }
        public DateTime DepDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
    }
}
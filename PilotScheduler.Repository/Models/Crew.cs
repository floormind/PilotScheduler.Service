namespace PilotScheduler.Service.Models.Classes
{
    public class Crew
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base { get; set; }
        public string[] WorkDays { get; set; }
    }
}
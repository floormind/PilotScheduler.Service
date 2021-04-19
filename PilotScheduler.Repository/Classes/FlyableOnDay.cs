using System;
using System.Collections.Generic;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Repository.Classes
{
    public class FlyableOnDay : IFlyableOnDay
    {
        public IList<Crew> CanFlyAtSchedule(DateTime departureDate, DateTime returnDate, Crew[] crews)
        {
            var availableToFly = new List<Crew>();
            foreach (var pilot in crews)
            {
                var departureDateIsAvailable = IsValidPilotWorkDay(departureDate, pilot.WorkDays);
                if (!departureDateIsAvailable)
                    continue;

                var returnDateIsAvailable = IsValidPilotWorkDay(returnDate, pilot.WorkDays);
                if (!returnDateIsAvailable)
                    continue;
                
                availableToFly.Add(pilot);
            }

            return availableToFly;
        }

        private bool IsValidPilotWorkDay(DateTime date, string[] workDays)
        {
            var dayOfWeek = date.DayOfWeek;
            var isAValidWorkDayForPilot = false;
            foreach (var workDay in workDays)
            {
                if (dayOfWeek.ToString() != workDay)
                    continue;
                isAValidWorkDayForPilot = true;
                break;
            }
            return isAValidWorkDayForPilot;
        }
    }
}
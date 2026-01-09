using TubeFeeding.Models;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        //public Food Food { get; set; }
        public Patient Patient { get; set; }
        public List<string> FormattedListOfHours { get; set; }
        public List<string> FormattedFeedingTimes { get; set; }
        public List<double> FeedingTimes { get; set; }
        public List<double> DayOne { get; set; }
        public List<double> DayTwo { get; set; }
        public List<double> DayThreeOnwards { get; set; }
        public List<List<string>> RefeedingSchedule { get; set; }

        public FeedingSchedule(Patient patient)
        {
            Patient = patient;
            FormattedFeedingTimes = [];
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.MealsPerDay);
            FormattedFeedingTimes = Globals.CreateFormattedListOfTimes(FeedingTimes);


        }
    }
}
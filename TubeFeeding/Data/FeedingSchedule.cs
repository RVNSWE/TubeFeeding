using TubeFeeding.Models;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        //public Food Food { get; set; }
        public Patient Patient { get; set; }
        public List<double> FeedingTimes { get; set; }
        public List<double> FeedingTimesDayOne { get; set; }
        public List<double> FeedingTimesDayTwo { get; set; }
        public List<string> FormattedFeedingTimes { get; set; }
        public List<string> FormattedFeedingTimesDayOne { get; set; }
        public List<string> FormattedFeedingTimesDayTwo { get; set; }

        public FeedingSchedule(Patient patient)
        {
            Patient = patient;
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.MealsPerDay);
            FeedingTimesDayOne = Globals.CalculateFeedingPlan(Patient.MealsPerDayOne);
            FeedingTimesDayTwo = Globals.CalculateFeedingPlan(Patient.MealsPerDayTwo);
            FormattedFeedingTimes = Globals.CreateFormattedListOfTimes(FeedingTimes);
            FormattedFeedingTimesDayOne = Globals.CreateFormattedListOfTimes(FeedingTimesDayOne);
            FormattedFeedingTimesDayTwo = Globals.CreateFormattedListOfTimes(FeedingTimesDayTwo);


        }
    }
}
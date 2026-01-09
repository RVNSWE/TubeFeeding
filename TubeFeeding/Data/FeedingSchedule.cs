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
        public List<double> ListOfHours {  get; set; }
        public List<double> FeedingTimes { get; set; }

        public FeedingSchedule(Patient patient)
        {
            Patient = patient;
            FormattedFeedingTimes = [];
            ListOfHours = [];
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.MealsPerDay);
            FormattedFeedingTimes = Globals.CreateFormattedListOfTimes(FeedingTimes);
        }
    }
}
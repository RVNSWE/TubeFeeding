using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        public PatientPageModel Patient { get; set; }
        public IReadOnlyCollection<double> FeedingTimes { get; set; }
        public IReadOnlyCollection<double> FeedingTimesDayOne { get; set; }
        public IReadOnlyCollection<double> FeedingTimesDayTwo { get; set; }
        public IReadOnlyCollection<string> FormattedFeedingTimes { get; set; }
        public IReadOnlyCollection<string> FormattedFeedingTimesDayOne { get; set; }
        public IReadOnlyCollection<string> FormattedFeedingTimesDayTwo { get; set; }

        public FeedingSchedule(PatientPageModel patient)
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
using TubeFeeding.Models;

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

            FormattedListOfHours = [];
            FormattedFeedingTimes = [];
            ListOfHours = [];
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.MealsPerDay);

            PopulateTimes();
        }

        public void PopulateTimes()
        {
            double time = FeedingTimes.First();

            while (time <= FeedingTimes.Max())
            {
                ListOfHours.Add(time);
                time++;

                if (time > 23)
                {
                    break;
                }
            }

            FormattedListOfHours = Globals.CreateFormattedListOfTimes(ListOfHours);
            FormattedListOfHours = Globals.CreateFormattedListOfTimes(FeedingTimes);
        }
    }
}
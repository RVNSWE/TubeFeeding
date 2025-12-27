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
            double startTime = FeedingTimes.First();
            double endTime = FeedingTimes.Last();
            double time = startTime;

            for (int i = 0; i < endTime; i++)
            {
                ListOfHours.Add(time);
                time++;
            }

            FormattedListOfHours = CreateFormattedList(ListOfHours, FormattedListOfHours);
            FormattedFeedingTimes = CreateFormattedList(FeedingTimes, FormattedFeedingTimes);
        }

        public List<string> CreateFormattedList(List<double> list, List<string> formattedList)
        {
            foreach (double time in list)
            {
                if (time < 10)
                {
                    formattedList.Add("0" + time.ToString() + ":00");
                }
                else
                {
                    formattedList.Add(time.ToString() + ":00");
                }
            }

            return formattedList;
        }
    }
}
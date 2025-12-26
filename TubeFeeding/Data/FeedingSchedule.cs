using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        //public Food Food { get; set; }
        public Patient Patient { get; set; }
        public List<string> FormattedListOfHours { get; set; }
        public List<string> FormattedFeedingTimes { get; set; }
        public List<int> ListOfHours {  get; set; }
        public List<int> FeedingTimes { get; set; }

        public FeedingSchedule(Patient patient)
        {
            Patient = patient;

            FormattedListOfHours = [];
            FormattedFeedingTimes = [];
            ListOfHours = [];
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.FoodPerDay, Patient.WaterPerDay, Patient.MaxTotalVolumePerMeal);

            PopulateTimes();
        }

        public void PopulateTimes()
        {
            int startTime = FeedingTimes.First();
            int endTime = FeedingTimes.Last();
            int time = startTime;

            for (int i = 0; i < endTime; i++)
            {
                ListOfHours.Add(time);
                time++;
            }

            CreateFormattedList(ListOfHours, FormattedListOfHours);
            CreateFormattedList(FeedingTimes, FormattedFeedingTimes);
        }

        public void CreateFormattedList(List<int> list, List<string> formattedList)
        {
            foreach (int time in list)
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
        }
    }
}
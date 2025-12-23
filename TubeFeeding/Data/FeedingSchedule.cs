namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        public FoodPageModel Food { get; set; }
        public PatientPageModel Patient { get; set; }
        public List<string> FormattedListOfHours { get; set; }
        public List<string> FormattedFeedingTimes { get; set; }
        public int[] ListOfHours {  get; set; }
        public int[] FeedingTimes { get; set; }

        public FeedingSchedule()
        {
            Food = App.SchedulePages?.SelectedFood;
            Patient = App.SchedulePages?.SelectedPatient;

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
                ListOfHours[i] = time;
                time++;
            }

            CreateFormattedList(ListOfHours, FormattedListOfHours);
            CreateFormattedList(FeedingTimes, FormattedFeedingTimes);
        }

        public void CreateFormattedList(int[] list, List<string> formattedList)
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
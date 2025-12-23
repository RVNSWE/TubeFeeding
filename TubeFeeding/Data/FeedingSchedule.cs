namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        public FoodPageModel Food { get; set; }
        public PatientPageModel Patient { get; set; }
        public List<string> Times { get; set; }
        public int[] FeedingTimes { get; set; }

        public FeedingSchedule()
        {
            Food = App.SchedulePages?.SelectedFood;
            Patient = App.SchedulePages?.SelectedPatient;

            Times = [];
            FeedingTimes = Globals.CalculateFeedingPlan(Patient.FoodPerDay, Patient.WaterPerDay, Patient.MaxTotalVolumePerMeal);

            PopulateTimes();
        }

        public void PopulateTimes()
        {
            foreach (int time in FeedingTimes)
            {
                if (time < 10)
                {
                    Times.Add("0" + time.ToString() + ":00");
                }
                else
                {
                    Times.Add(time.ToString() + ":00");
                }
            }
        }
    }
}
using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        public Food Food { get; set; }
        public Patient Patient { get; set; }
        public List<string> Times { get; set; }
        public int[] FeedingTimes { get; set; }

        public FeedingSchedule(int[] feedingTimes)
        {
            Food = new();
            Patient = new();

            Times = [];
            FeedingTimes = feedingTimes;
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
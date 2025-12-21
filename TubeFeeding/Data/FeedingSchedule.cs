using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    public class FeedingSchedule
    {
        public Food Food { get; set; }
        public Patient Patient { get; set; }
        public int[] FeedingTimes { get; set; }

        public FeedingSchedule(int[] feedingTimes)
        {
            Food = new();
            Patient = new();

            FeedingTimes = feedingTimes;
        }
    }
}
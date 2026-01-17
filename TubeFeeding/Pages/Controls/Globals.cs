namespace TubeFeeding.Pages.Controls
{
    public static class Globals
    {
        public const string DatabaseName = "TF.db3";

        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ProtectionComplete;

        public static string GetLocalPath(string fileName)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
        }

        /*
         * Converts a null binding value into an empty string.
         */
        public static string FormatString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }
            else
            {
                value = value.Trim();
            }

            return value;
        }

        public static bool IsStringEmpty(string value)
        {
            if (value == "")
            {
                return true;
            }
            return false;
        }

        public static double CalculateRER(double bodyWeight, string species)
        {
            double rER;

            if (species == "Cat")
            {
                rER = bodyWeight * 30 + 70;
            }
            else
            {
                rER = 70 * Math.Pow(bodyWeight, 0.75);
            }

            return rER;
        }

        public static double CalculateInitialWaterPerDay(double minWaterPerDay, double maxWaterPerDay)
        {
            if (minWaterPerDay < 0)
            {
                if (maxWaterPerDay < 0)
                {
                    return 0;
                }
                return maxWaterPerDay;
            }
            return minWaterPerDay;
        }

        public static double MinFluidCalculation(int multiplier, double bodyWeight)
        {
            double resultOneTotalFluidsPerDay = multiplier * bodyWeight;

            return resultOneTotalFluidsPerDay;
        }

        public static double MaxFluidCalculation(int multiplier, double bodyWeight)
        {
            double resultTwoTotalFluidsPerDay = multiplier * Math.Pow(bodyWeight, 0.75);

            return resultTwoTotalFluidsPerDay;
        }

        public static double GetFlushPerMeal(double bodyWeight)
        {
            double flushPerMeal = bodyWeight switch
            {
                < 1.5 => 2,
                < 2 => 3,
                < 3 => 4,
                < 4 => 5,
                < 4.5 => 6,
                < 5 => 8,
                < 8 => 10,
                < 20 => 12,
                _ => 20,
            };

            return flushPerMeal;
        }

        /*
         * Calculate the time interval between feeds.
         */
        public static double CalculateInterval(double mealsPerDay)
        {
            int hours = 15; // Number of hours to spread the feeds over

            double preciseInterval = hours / mealsPerDay;
            double interval = Math.Round(preciseInterval / 5, 1, MidpointRounding.AwayFromZero) * 5; // To the nearest 5 = to the nearest half hour
            
            if (interval < 1)
            {
                interval = 1; // Minimum feeding interval is always 1 hour
            }

            return interval;
        }

        public static List<double> CalculateFeedingPlan(double mealsPerDay)
        {
            double interval = CalculateInterval(mealsPerDay);

            double preciseMealHalfTime = (mealsPerDay * interval) / 2; // Effectively hours / 2
            double mealHalfTime = Math.Round(preciseMealHalfTime / 5, 1, MidpointRounding.AwayFromZero) * 5;
            int midPoint = 16; // Corresponding to 16:00 or 4pm
            double startTime = midPoint - mealHalfTime;
            double endTime = startTime + mealHalfTime;

            while (endTime > 23.5)
            {
                midPoint -= 1;
                startTime = midPoint - mealHalfTime;
                endTime = midPoint + mealHalfTime;
            }

            double time = startTime;

            List<double> feedingTimes = [];

            if (interval > 1)
            {
                for (int i = 0; i < mealsPerDay; i++)
                {
                    feedingTimes.Add(time);
                    time += interval;
                }
            }
            else if (mealsPerDay > 23)
            {
                time = 0;
                for (int i = 0; i < 24; i++)
                {
                    feedingTimes.Add(time);
                    time++;
                }
            }
            else
            {
                for (int i = 0; i < mealsPerDay; i++)
                {
                    feedingTimes.Add(time);
                    time++;
                }
            }

                return feedingTimes;
        }

        public static List<string> CreateFormattedListOfTimes(List<double> list)
        {
            List<string> formattedList = [];

            foreach (double time in list)
            {
                int roundedTime = (int)Math.Round(time, 0, MidpointRounding.AwayFromZero);
                string formattedTime;

                string hours = time.ToString();
                string minutes = ":00";

                if (time < roundedTime)
                {
                    hours = Math.Round(time, 0, MidpointRounding.ToZero).ToString();
                    minutes = ":30";
                }

                if (time < 10)
                {
                    hours = "0" + Math.Round(time, 0, MidpointRounding.ToZero).ToString();
                }

                formattedTime = hours + minutes;

                formattedList.Add(formattedTime);
            }

            return formattedList;
        }

        /*
         * Navigate to the 'list' Path of the current Route.
         */
        public static async void GoToList()
        {
            await Shell.Current.GoToAsync("list");
        }

        /*
         * Navigate to the 'add' Path of the current Route.
         */
        public static async void GoToAdd()
        {
            await Shell.Current.GoToAsync("add");
        }

        /*
         * Navigate to the 'view' Path of the current Route.
         */
        public static async void GoToView()
        {
            await Shell.Current.GoToAsync("view");
        }
    }
}
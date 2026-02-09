namespace TubeFeeding.Pages.Controls
{
    public static class Globals
    {
        public const string DatabaseName = "TF.db3";

        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ProtectionComplete;

        public static string GetLocalPath(string fileName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
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

        /*
         * Check if string is empty.
         */
        public static bool IsStringEmpty(string value)
        {
            if (value == "")
            {
                return true;
            }
            return false;
        }

        /*
         * Calculate resting energy requirement.
         */
        public static double CalculateRER(double bodyWeight)
        {
            double rER;

            rER = 70 * Math.Pow(bodyWeight, 0.75); // Use the more accurate universal calculation.

            return rER;
        }

        /*
         * Calculate estimated total required fluid input per day using simple fluid calculation.
         */
        public static double FluidCalculationOne(int multiplier, double bodyWeight)
        {
            double totalFluidsPerDay = multiplier * bodyWeight;

            return totalFluidsPerDay;
        }

        /*
         * Calculate estimated total required fluid input per day using more complex calculation.
         */
        public static double FluidCalculationTwo(int multiplier, double bodyWeight)
        {
            double totalFluidsPerDay = multiplier * Math.Pow(bodyWeight, 0.75);

            return totalFluidsPerDay;
        }

        /*
         * Calculate fluids provided by food and return the minimum amount of additional water to administer.
         */
        public static double GetMinWaterPerDay(double totalFluidsPerDayCalcOne, double totalFluidsPerDayCalcTwo, double foodWaterContent)
        {
            double minWaterPerDay;

            if (totalFluidsPerDayCalcOne < totalFluidsPerDayCalcTwo) // Use the calculation that provides the lowest estimate
            {
                minWaterPerDay = totalFluidsPerDayCalcOne - foodWaterContent;
            }
            else
            {
                minWaterPerDay = totalFluidsPerDayCalcTwo - foodWaterContent;
            }

            if (minWaterPerDay < 0) // If the food provides more than the minimum requirement already
            {
                minWaterPerDay = 0; // Set additional water to zero
            }

            return minWaterPerDay;
        }

        /*
         * If bodyweight is greater than value, check next value. Stop at first value lower than bodyweight, and return output as total volume
         * of flush to be adminstered per meal.
         */
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
            double interval = Math.Round(preciseInterval / 5, 1, MidpointRounding.AwayFromZero) * 5; // Round to the nearest 5 = to the nearest half hour
            
            if (interval < 1) // If interval less than one hour
            {
                interval = 1; // Set feeding interval to one hour (constraint: interval can never be less than an hour)
            }

            return interval;
        }

        /*
         * Calculate the first and last feeding times of the day from an initial assumed midpoint of 16:00 (4pm).
         */
        public static List<double> CalculateFeedingPlan(double mealsPerDay)
        {
            double interval = CalculateInterval(mealsPerDay);

            double preciseMealHalfTime = (mealsPerDay * interval) / 2; // Effectively half the total number of hours to adminster feeds over per day
            double mealHalfTime = Math.Round(preciseMealHalfTime / 5, 1, MidpointRounding.AwayFromZero) * 5; // Round to the nearest 5
            int midPoint = 16; // Corresponding to 16:00 or 4pm
            double startTime = midPoint - mealHalfTime; // Calculate the feeding schedule start time from its mid point
            double endTime = startTime + mealHalfTime; // Calculate the end time from the mid point

            while (endTime > 23.5) // While current end time is later than 23:30
            {
                midPoint -= 1; // Shift the mid point one hour earlier
                startTime = midPoint - mealHalfTime; // Recalculate the start time
                endTime = midPoint + mealHalfTime; // Recalculate the end time
            }

            double time = startTime; // Start from the calculated start time

            List<double> feedingTimes = []; // Initialise the output list

            if (interval > 1) // If the interval is longer than 1 hour
            {
                for (int i = 0; i < mealsPerDay; i++) // For each meal to be scheduled
                {
                    feedingTimes.Add(time); // Add this time to the list
                    time += interval; // Increment the time by the calculated interval
                }
            }
            else if (mealsPerDay > 23) // Otherwise, if the patient needs more than 23 meals per day
            {
                time = 0; // Set the time to midnight
                for (int i = 0; i < 24; i++) // For each meal (which will be every hour)
                {
                    feedingTimes.Add(time); // Add the current time to the list
                    time++; // Increment time by one hour
                }
            }
            else // Otherwise
            {
                for (int i = 0; i < mealsPerDay; i++) // For each meal to be scheduled
                {
                    feedingTimes.Add(time); // Add this time to the list
                    time++; // Increment the time by one hour
                }
            }

                return feedingTimes;
        }

        /*
         * Convert the calculated times into a human readable list of times
         */
        public static List<string> CreateFormattedListOfTimes(IReadOnlyCollection<double> list)
        {
            List<string> formattedList = []; // Initialise output list

            foreach (double time in list) // For each time in the calculated feeding schedule
            {
                int roundedTime = (int)Math.Round(time, 0, MidpointRounding.AwayFromZero); // Round the time to the nearest int
                string formattedTime; // Prepare a string field for the formatted (human readable) time

                string hours = time.ToString(); // Store the hour as a string
                string minutes = ":00"; // Set the minutes to 0

                if (time < roundedTime) // If the real time is less than the rounded time then it was rounded upwards, meaning minutes were 30+
                {
                    hours = Math.Round(time, 0, MidpointRounding.ToZero).ToString(); // So round the hour down
                    minutes = ":30"; // And set minutes to 30
                }

                if (time < 10) // If time is less than 10 then the hour is only one digit long
                {
                    hours = "0" + Math.Round(time, 0, MidpointRounding.ToZero).ToString(); // So add a zero before it
                }

                formattedTime = hours + minutes; // Combine the hours and minutes into one string

                formattedList.Add(formattedTime); // Add the human readable time to the output list
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
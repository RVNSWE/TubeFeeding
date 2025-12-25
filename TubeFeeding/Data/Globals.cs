namespace TubeFeeding.Data
{
    public static class Globals
    {
        public const string DatabaseName = "DAR.db3";

        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ProtectionComplete;

        /*
         * Shorten a filepath name.
         */
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

        /*
         * Check whether a string is empty.
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
         * Check the phone number has no whitespace or non-digit characters.
         * 
         * TO DO: For usability, should be able to enter whitespace between numbers and exclude this from both the
         * character limit and validity checking.
         */
        /*public static bool ValidatePhoneNumber(string phoneNumber)
        {
            foreach (char character in phoneNumber)
            {
                if (character < '0' || character > '9')
                    return false;
            }

            if (phoneNumber.Length != 11)
                return false;

            return true;
        }*/

        /*
         * Check whether a valid time has been entered.
         */
        /*public static bool ValidateTime(string time)
        {
            foreach (char character in time)
            {
                if (character < '0' || character > '9')
                    return false;
            }

            if (time.Length != 2)
                return false;

            return true;
        }*/

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

        public static double CalculateFluidsPerDay(double bodyWeight)
        {
            double fluidsPerDay = 2 * bodyWeight * 24;

            return fluidsPerDay;
        }

        public static double CalculateKcalPerGram(double kcal, double netWeight)
        {
            double kcalPerGram = kcal * 0.001;

            return kcalPerGram;
        }

        public static double CalculateWaterContent(double netWeight, double waterPercentage)
        {
            double waterDecimal = waterPercentage / 100;
            double waterContent = netWeight * waterDecimal;

            return waterContent;
        }

        public static double CalculateMaxTotalVolumePerMeal(double bodyWeight, int MAX_ML_PER_KG)
        {
            double maxTotalVolumePerMeal = bodyWeight * MAX_ML_PER_KG;

            return maxTotalVolumePerMeal;
        }

        public static double CalculateFoodPerDay(double rER, double kcalPerGram)
        {
            double foodPerDay = rER / kcalPerGram;

            return foodPerDay;
        }

        public static double CalculateFoodPerMeal(double foodPerDay, int mealsPerDay)
        {
            double foodPerMeal = foodPerDay / mealsPerDay;

            return foodPerMeal;
        }

        public static double CalculateWaterPerDay(double fluidsPerDayTotal, double foodPerDay, double waterPercentage)
        {
            double waterContent = CalculateWaterContent(foodPerDay, waterPercentage);
            double waterPerDay = fluidsPerDayTotal - waterContent;

            return waterPerDay;
        }

        public static double CalculateWaterPerMeal(double waterPerDay, int mealsPerDay)
        {
            double waterPerMeal = waterPerDay / mealsPerDay;

            return waterPerMeal;
        }

        public static List<int> CalculateFeedingPlan(double foodPerDay, double waterPerDay, double maxTotalVolumePerMeal)
        {
            double totalVolumePerDay = foodPerDay + waterPerDay;
            double mealsPerDay = totalVolumePerDay / maxTotalVolumePerMeal;
            mealsPerDay = Math.Round(mealsPerDay, 0, MidpointRounding.ToPositiveInfinity);
            List<int> feedingTimes = [];
            int hours = 14;
            double interval = hours / mealsPerDay;
            interval = Math.Round(interval, 0, MidpointRounding.ToZero);

            if (interval < 1)
            {
                interval = 1;
            }

            double mealHalfTime = mealsPerDay * interval / 2;
            mealHalfTime = Math.Round(mealHalfTime, 0, MidpointRounding.ToPositiveInfinity);
            int midPoint = 15;
            int startTime = midPoint - (int)mealHalfTime;
            int increment = 0;
            int time = 0;

            if (mealsPerDay < 14)
            {
                for (int i = 0; i < mealsPerDay; i++)
                {
                    time = startTime + increment;
                    feedingTimes.Add(time);
                    increment += (int)interval;
                }
            }
            if (mealsPerDay > 23)
            {
                for (int i = 0; i < 24; i++)
                {
                    feedingTimes.Add(time);
                    time++;
                }
            }
            else
            {
                time = midPoint - (int)mealHalfTime;

                for (int i = 0; i < mealsPerDay; i++)
                {
                    feedingTimes.Add(time);
                    time++;
                }
            }

                return feedingTimes;
        }

        /*
         * Go back to the schedule list.
         */
        public static async void GoToSchedule()
        {
            await Shell.Current.GoToAsync("//schedule");
        }

        /*
         * Go back to the food list.
         */
        public static async void GoToFood()
        {
            await Shell.Current.GoToAsync("//food");
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

        /*
         * Navigate to the 'edit' Path of the current Route.
         */
        public static async void GoToEdit()
        {
            await Shell.Current.GoToAsync("edit");
        }

        /*
         * Refresh the lists.
         */
        public static async void RefreshSchedules()
        {
            await App.SchedulePages?.RefreshPatients();
        }

        /*
         * Validate an integer.
         */
        /*public static bool ValidateInt(string userInput)
        {
            if (int.TryParse(userInput, out var number))
            {
                return true;
            }

            return false;
        }*/
    }
}
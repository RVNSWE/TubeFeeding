namespace TubeFeeding
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

        public static double CalculateKcalPerGram(double kcal, double netWeight)
        {
            double kcalPerGram = kcal / netWeight;

            return kcalPerGram;
        }

        public static double CalculateWaterContent(double netWeight, double dryWeight)
        {
            double waterContent = netWeight - dryWeight;

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

        public static double CalculateWaterPerDay(double fluidsPerDayTotal, double waterContent)
        {
            double waterPerDay = fluidsPerDayTotal - waterContent;

            return waterPerDay;
        }

        public static double CalculateWaterPerMeal(double waterPerDay, int mealsPerDay)
        {
            double waterPerMeal = waterPerDay / mealsPerDay;

            return waterPerMeal;
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
            await App.SchedulePages?.RefreshSchedules();
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
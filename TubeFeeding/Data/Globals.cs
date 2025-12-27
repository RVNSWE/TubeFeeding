using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    public static class Globals
    {
        public const string DatabaseName = "TF.db3";

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

        public static List<int> CalculateFeedingPlan(double mealsPerDay)
        {
            List<int> feedingTimes = [];
            int midPoint = 15; // Corresponding to 15:00 or 3pm
            int hours = 14; // Over a period of n hours

            double preciseInterval = hours / mealsPerDay;
            double interval = Math.Round(preciseInterval / 5, 1, MidpointRounding.AwayFromZero) * 5; // To the nearest 5 = to the nearest half hour
            if (interval < 1)
            {
                interval = 1; // Minimum feeding interval is 1 hour
            }

            // TO DO: Do as with interval - get 30 min increments
            double mealHalfTime = Math.Round(mealsPerDay / 2 * interval, 0, MidpointRounding.AwayFromZero); // Effectively hours / 2
            int startTime = midPoint - (int)mealHalfTime;
            int endTime = midPoint + (int)mealHalfTime;
            System.Diagnostics.Debug.WriteLine("startTime: " + startTime);
            System.Diagnostics.Debug.WriteLine("endTime: " + endTime);

            while (endTime > 23)
            {
                midPoint -= 1;
                startTime = midPoint - (int)mealHalfTime;
                endTime = midPoint + (int)mealHalfTime;
                System.Diagnostics.Debug.WriteLine("startTime: " + startTime);
                System.Diagnostics.Debug.WriteLine("endTime: " + endTime);
            }

            int increment = 0;
            int time = 0;

            if (mealsPerDay < midPoint)
            {
                for (int i = 0; i < mealsPerDay; i++)
                {
                    time = startTime + increment;
                    feedingTimes.Add(time);
                    increment += (int)Math.Round(interval, 0, MidpointRounding.AwayFromZero);
                }
            }
            else if (mealsPerDay > 23)
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

        public static void CreateFormattedList(List<int> list, List<string> formattedList)
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
            await App.PatientPage?.RefreshPatients();
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
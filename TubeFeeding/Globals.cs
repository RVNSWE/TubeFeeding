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
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            foreach (char character in phoneNumber)
            {
                if (character < '0' || character > '9')
                    return false;
            }

            if (phoneNumber.Length != 11)
                return false;

            return true;
        }

        /*
         * Check whether a valid time has been entered.
         */
        public static bool ValidateTime(string time)
        {
            foreach (char character in time)
            {
                if (character < '0' || character > '9')
                    return false;
            }

            if (time.Length != 2)
                return false;

            return true;
        }

        /*
         * Go back to the patient list.
         */
        public static async void GoToPatient()
        {
            await Shell.Current.GoToAsync("//patient");
        }

        /*
         * Go back to the chart list.
         */
        public static async void GoToChart()
        {
            await Shell.Current.GoToAsync("//chart");
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
        public static async void RefreshPatients()
        {
            await App.PatientViewModel?.RefreshPatients();
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
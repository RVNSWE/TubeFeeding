using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
using TubeFeeding.Models;

namespace TubeFeeding.Pages.Controls
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

        public static async Task CreatePDF()
        {
            int patientId;

            if (App.PatientPage?.SelectedPatient == null)
            {
                patientId = App.PatientPage.LastPatientSelected.Id;
            }
            else
            {
                patientId = App.PatientPage.SelectedPatient.Id;
            }

            System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient ID {patientId}");

            try
            {
                Patient patient = await App.Repo.GetPatient(patientId);

                string pdfPath = GetLocalPath($"{patient.PatientName}_{patient.ClientName}_{patient.FoodName}.pdf");
                FeedingSchedule feedingSchedule = new(patient);
                ExportDoc output = new(feedingSchedule, pdfPath);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = $"{patient.PatientName} {patient.ClientName} - Tube Feeding Plan",
                    File = new ShareFile(pdfPath)
                });

                System.Diagnostics.Debug.WriteLine("PDF creation successful.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not create PDF. Error: " + ex.Message);
            }
        }

        async static Task SaveFile(CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));
            var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, cancellationToken);
            if (fileSaverResult.IsSuccessful)
            {
                await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
            }
            else
            {
                await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
            }
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
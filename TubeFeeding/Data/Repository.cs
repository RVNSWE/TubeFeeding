using SQLite;
using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    /*
     * Class for managing SQLite database interactions.
     */
    public class Repository
    {
        string _dbPath; // database pdf location

        private SQLiteAsyncConnection conn;

        /*
         * Initialise the SQLite database. Only needs to happen once, so return immediately if
         * this has already been done.
         */
        private async Task Init()
        {
            if (conn != null) // if a database connection has already been made,
                return; // exit function

            // otherwise
            conn = new SQLiteAsyncConnection(_dbPath); // connect to the database via the specified filepath
            await conn.CreateTableAsync<Patient>();
        }

        /*
         * Initialise the database filepath.
         */
        public Repository(string dbPath)
        {
            _dbPath = dbPath;
        }

        /*
         * FOR DEBUGGING - Drop the current Patient table.
         */
        public async Task DropPatientTable()
        {
            System.Diagnostics.Debug.WriteLine("Attempting to drop patient table");

            try
            {
                await conn.DropTableAsync<Patient>();
                System.Diagnostics.Debug.WriteLine("Dropping patient table");
                await conn.CreateTableAsync<Patient>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }
        }

        /*
         * Add a new patient.
         */
        public async Task AddNewPatient(
            string foodName,
            double kcalPerMl,
            double waterContent,
            string patientName,
            string clientName,
            string species,
            double bodyWeight,
            double maxTotalVolumePerMeal,
            double maxTotalVolumePerMealDayOne,
            double maxTotalVolumePerMealDayTwo,
            double foodPerMeal,
            double foodPerMealDayOne,
            double foodPerMealDayTwo,
            double flushPerMeal,
            double waterToAddPerMeal,
            double waterToAddPerMealDayOne,
            double waterToAddPerMealDayTwo,
            int mealsPerDay,
            int mealsPerDayOne,
            int mealsPerDayTwo,
            double cansPerDay,
            double cansPerDayOne,
            double cansPerDayTwo
            )
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add patient");

            Patient patient = new()
            {
                FoodName = foodName,
                KcalPerMl = kcalPerMl,
                WaterContent = waterContent,
                PatientName = patientName,
                ClientName = clientName,
                Species = species,
                BodyWeight = bodyWeight,
                MaxTotalVolumePerMeal = maxTotalVolumePerMeal,
                MaxTotalVolumePerMealDayOne = maxTotalVolumePerMealDayOne,
                MaxTotalVolumePerMealDayTwo = maxTotalVolumePerMealDayTwo,
                FoodPerMeal = foodPerMeal,
                FoodPerMealDayOne = foodPerMealDayOne,
                FoodPerMealDayTwo = foodPerMealDayTwo,
                VolPerFlush = flushPerMeal,
                WaterToAddPerMeal = waterToAddPerMeal,
                WaterToAddPerMealDayOne = waterToAddPerMealDayOne,
                WaterToAddPerMealDayTwo = waterToAddPerMealDayTwo,
                MealsPerDay = mealsPerDay,
                MealsPerDayOne = mealsPerDayOne,
                MealsPerDayTwo = mealsPerDayTwo,
                CansPerDay = cansPerDay,
                CansPerDayOne = cansPerDayOne,
                CansPerDayTwo = cansPerDayTwo
            };

            int result;
            try
            {
                await Init();

                result = await conn.InsertAsync(patient);

                System.Diagnostics.Debug.WriteLine("{0} patient added (record ID: {1})", result, patient.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not add patient record ID {0}. Error: {1}", patient.Id, ex.Message);
            }

            await App.PatientPage?.UpdatePatients(patient);
        }

        /*
         * Update the currently selected patient.
         */
        public async Task UpdatePatient(
            int id,
            string foodName,
            double kcalPerMl,
            double waterContent,
            string patientName,
            string clientName,
            string species,
            double bodyWeight,
            double maxTotalVolumePerMeal,
            double maxTotalVolumePerMealDayOne,
            double maxTotalVolumePerMealDayTwo,
            double foodPerMeal,
            double foodPerMealDayOne,
            double foodPerMealDayTwo,
            double flushPerMeal,
            double waterToAddPerMeal,
            double waterToAddPerMealDayOne,
            double waterToAddPerMealDayTwo,
            int mealsPerDay,
            int mealsPerDayOne,
            int mealsPerDayTwo,
            double cansPerDay,
            double cansPerDayOne,
            double cansPerDayTwo
            )
        {
            Patient patient = new()
            {
                Id = id,
                FoodName = foodName,
                KcalPerMl = kcalPerMl,
                WaterContent = waterContent,
                PatientName = patientName,
                ClientName = clientName,
                Species = species,
                BodyWeight = bodyWeight,
                MaxTotalVolumePerMeal = maxTotalVolumePerMeal,
                MaxTotalVolumePerMealDayOne = maxTotalVolumePerMealDayOne,
                MaxTotalVolumePerMealDayTwo = maxTotalVolumePerMealDayTwo,
                FoodPerMeal = foodPerMeal,
                FoodPerMealDayOne = foodPerMealDayOne,
                FoodPerMealDayTwo = foodPerMealDayTwo,
                VolPerFlush = flushPerMeal,
                WaterToAddPerMeal = waterToAddPerMeal,
                WaterToAddPerMealDayOne = waterToAddPerMealDayOne,
                WaterToAddPerMealDayTwo = waterToAddPerMealDayTwo,
                MealsPerDay = mealsPerDay,
                MealsPerDayOne = mealsPerDayOne,
                MealsPerDayTwo = mealsPerDayTwo,
                CansPerDay = cansPerDay,
                CansPerDayOne = cansPerDayOne,
                CansPerDayTwo = cansPerDayTwo
            };

            await conn.UpdateAsync(patient);

            await App.PatientPage?.UpdatePatients(patient);
        }

        /*
         * Get a list of all patients.
         */
        public async Task<List<Patient>> GetAllPatients()
        {
            try
            {
                await Init();
                return await conn.Table<Patient>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Patient>();
        }

        /*
         * Get a specific patient by ID.
         */
        public async Task<Patient> GetPatient(int id)
        {
            Patient schedule = new();

            try
            {
                await Init();

                schedule = await conn.FindAsync<Patient>(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return schedule;
        }

        /*
         * Delete a patient.
         */
        public async Task DeletePatient(PatientPageModel patientPageModel)
        {
            int result = 0;
            try
            {
                await Init();

                App.PatientPage?.ForceSelectPatient(patientPageModel);

                Patient thisPatient = await conn.Table<Patient>().Where(i => i.Id == patientPageModel.Id).FirstOrDefaultAsync();
                string name = thisPatient.PatientName;

                App.PatientPage?.Patients.Remove(patientPageModel);
                result = await conn.DeleteAsync(thisPatient);
                await App.PatientPage?.RefreshPatients();

                System.Diagnostics.Debug.WriteLine("{0} patient(s) deleted (patient: {1})", result, name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not delete patient. Error: {1}", ex.Message);
            }
        }
    }
}
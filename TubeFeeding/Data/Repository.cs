using SQLite;
using TubeFeeding.Models;

namespace TubeFeeding.Data
{
    public class Repository
    {
        string _dbPath;

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
         * Add a new patient.
         */
        public async Task AddNewSchedule(Patient patient)
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add patient");

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
        public async Task UpdateSchedule(Patient patient)
        {
            await conn.UpdateAsync(patient);

            await App.PatientPage?.UpdatePatients(patient);
        }

        /*
         * Get a list of all patients.
         */
        public async Task<List<Patient>> GetAllSchedules()
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
        /*public async Task<Patient> GetSchedule(int id)
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
        }*/

        /*
         * Delete a patient.
         */
        public async Task DeleteSchedule(PatientPageModel patientPageModel)
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
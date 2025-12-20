using TubeFeeding.Models;
using TubeFeeding.PageModels;
using QuestPDF.Fluent;
using SQLite;

namespace TubeFeeding.Clients
{
    /*
     * Class for managing SQLite database interactions.
     */
    public class Repository
    {
        string _dbPath; // database pdf location

        public string StatusMessage { get; set; } // feedback message for the user

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
            await conn.CreateTableAsync<Food>();
            await conn.CreateTableAsync<Schedule>();
        }

        /*
         * Initialise the database filepath.
         */
        public Repository(string dbPath)
        {
            _dbPath = dbPath;
        }

        /*
         * Output a chart as a PDF.
         */
        public async Task OutputChart()
        {
            try
            {
                Patient patient = await conn.FindAsync<Patient>(App.PatientViewModel?.SelectedPatient.Id);
                Chart chart = await conn.FindAsync<Chart>(App.PatientViewModel?.LastPatientSelected.SelectedChart.Id);

                int recordId = chart.Id;

                PreAnaes preAnaesthesia = await conn.Table<PreAnaes>().FirstOrDefaultAsync(c => c.RecordIdPKey == recordId);
                IntraAnaes intraAnaesthesia = await conn.Table<IntraAnaes>().FirstOrDefaultAsync(c => c.RecordIdPKey == recordId);
                PostAnaes postAnaesthesia = await conn.Table<PostAnaes>().FirstOrDefaultAsync(c => c.RecordIdPKey == recordId);
                Drug drug = await conn.Table<Drug>().FirstOrDefaultAsync(c => c.RecordIdPKey == recordId);
                Fluid fluid = await conn.Table<Fluid>().FirstOrDefaultAsync(c => c.RecordIdPKey == recordId);
                List<MonitorParams> parameters = await conn.Table<MonitorParams>().Where(c => c.RecordIdPKey == recordId).ToListAsync();

                AnaesRecord anaesthesiaRecord = new AnaesRecord
                {
                    Patient = patient,
                    Chart = chart,
                    PreAnaes = preAnaesthesia,
                    IntraAnaes = intraAnaesthesia,
                    PostAnaes = postAnaesthesia,
                    Drug = drug,
                    Fluid = fluid,
                    MonitorParams = parameters
                };

                ExportDoc output = new ExportDoc(anaesthesiaRecord);
                string pdf = Globals.GetLocalPath($"{patient.PatientName}_{patient.ClientName}_{chart.Procedure}_{chart.Id}.pdf");
                output.GeneratePdf(pdf);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = $"{patient.PatientName} {patient.ClientName} {chart.Procedure} Anaesthesia Record",
                    File = new ShareFile(pdf)
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }
        }

        /*
         * FOR DEBUGGING ONLY - Drop the current Patient table.
         */
        public async Task DropPatientTable()
        {
            System.Diagnostics.Debug.WriteLine("Attempting to drop Patient table");

            try
            {
                await conn.DropTableAsync<Patient>();
                System.Diagnostics.Debug.WriteLine("Dropping Patient table");
                await conn.CreateTableAsync<Patient>(); // create a table for storing Patient data
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            await DropChartTables();
        }

        /*
         * FOR DEBUGGING ONLY - Drop the current chart tables.
         */
        public async Task DropChartTables()
        {
            System.Diagnostics.Debug.WriteLine("Attempting to drop chart tables");

            try
            {
                await conn.DropTableAsync<Chart>();
                System.Diagnostics.Debug.WriteLine("Dropping chart table");
                await conn.CreateTableAsync<Chart>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {

                await conn.DropTableAsync<PreAnaes>();
                System.Diagnostics.Debug.WriteLine("Dropping pre an table");
                await conn.CreateTableAsync<PreAnaes>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {

                await conn.DropTableAsync<IntraAnaes>();
                System.Diagnostics.Debug.WriteLine("Dropping intra an table");
                await conn.CreateTableAsync<IntraAnaes>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {
                await conn.DropTableAsync<PostAnaes>();
                System.Diagnostics.Debug.WriteLine("Dropping post an table");
                await conn.CreateTableAsync<PostAnaes>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {
                await conn.DropTableAsync<Drug>();
                System.Diagnostics.Debug.WriteLine("Dropping drugsfluids table");
                await conn.CreateTableAsync<Drug>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {
                await conn.DropTableAsync<Fluid>();
                System.Diagnostics.Debug.WriteLine("Dropping drugsfluids table");
                await conn.CreateTableAsync<Fluid>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            try
            {
                await conn.DropTableAsync<MonitorParams>();
                System.Diagnostics.Debug.WriteLine("Dropping params table");
                await conn.CreateTableAsync<MonitorParams>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }
        }

        /*
         * Add a new Patient to the Patient table.
         */
        public async Task AddNewPatient(
            string clientId,
            string clientName,
            string phoneNumber,
            string patientId,
            string patientName,
            string species,
            string age,
            string sex,
            string breed,
            string neuteredStatus,
            string temperament,
            double weight
            )
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add Patient");

            Patient patient = new()
            {
                ClientId = clientId,
                ClientName = clientName,
                PhoneNumber = phoneNumber,
                PatientId = patientId,
                PatientName = patientName,
                Species = species,
                Age = age,
                Sex = sex,
                Breed = breed,
                NeuteredStatus = neuteredStatus,
                Temperament = temperament,
                Weight = weight
            };

            int result = 0;

            try
            {
                await Init();

                result = await conn.InsertAsync(patient);

                StatusMessage = string.Format("{0} Patient(s) added (name: {1})", result, patientName);
                System.Diagnostics.Debug.WriteLine("{0} Patient(s) added (name: {1})", result, patientName);

                await App.PatientViewModel?.UpdatePatients(patient);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add Patient {0}. Error: {1}", patientName, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not add Patient {0}. Error: {1}", patientName, ex.Message);
            }
        }

        /*
         * Add a new chart to the Chart table.
         */
        public async Task AddNewChart(
            int patientIdPKey,
            string date,
            string anaesthetist,
            string clinician,
            string procedure
            )
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add chart");

            Chart chart = new()
            {
                PatientIdPKey = patientIdPKey,
                Date = date,
                Anaesthetist = anaesthetist,
                Clinician = clinician,
                Procedure = procedure
            };

            int result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(chart);

                StatusMessage = string.Format("{0} procedure details added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} procedure details added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add procedure details record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add procedure details record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            PreAnaes preAn = new()
            {
                RecordIdPKey = chart.Id
            };

            IntraAnaes intraAn = new()
            {
                RecordIdPKey = chart.Id
            };

            PostAnaes postAn = new()
            {
                RecordIdPKey = chart.Id
            };

            Drug drug = new()
            {
                RecordIdPKey = chart.Id
            };

            Fluid fluid = new()
            {
                RecordIdPKey = chart.Id
            };

            MonitorParams monitorParams = new()
            {
                RecordIdPKey = chart.Id,
            };

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(preAn);

                StatusMessage = string.Format("{0} pre anaes added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} pre anaes added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add pre anaes record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add pre anaes record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(intraAn);

                StatusMessage = string.Format("{0} intra anaes added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} intra anaes added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add intra anaes record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add intra anaes record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(postAn);

                StatusMessage = string.Format("{0} post anaes added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} post anaes added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add post anaes record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add post anaes record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(drug);

                StatusMessage = string.Format("{0} drugsfluids added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} drugsfluids added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add drugsfluids record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add drugsfluids record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(fluid);

                StatusMessage = string.Format("{0} drugsfluids added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} drugsfluids added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add drugsfluids record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add drugsfluids record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(monitorParams);

                StatusMessage = string.Format("{0} parameters added (record ID: {1})", result, chart.Id);

                System.Diagnostics.Debug.WriteLine("{0} parameters added (record ID: {1})", result, chart.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add parameters record ID {0}. Error: {1}", chart.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add parameters record ID {0}. Error: {1}", chart.Id, ex.Message);
            }

            await App.PatientViewModel?.UpdateCharts(chart);
        }

        /*
         * Update a chart.
         */
        public async Task UpdateChart(
            int patientIdPKey,
            string date,
            string anaesthetist,
            string clinician,
            string procedure
            )
        {
            Chart chart = await conn.FindAsync<Chart>(patientIdPKey);
            //PreAnaesthesia chart = await conn.Table<PreAnaesthesia>().Where(i => i.RecordIdPKey == recordIdPKey).FirstOrDefaultAsync();

            chart.Date = date;
            chart.Anaesthetist = anaesthetist;
            chart.Clinician = clinician;
            chart.Procedure = procedure;

            int result = 0;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update pre-anaesthesia details");

                result = await conn.UpdateAsync(chart);

                StatusMessage = string.Format("{0} pre-an updated (record ID: {1})", result, patientIdPKey);

                System.Diagnostics.Debug.WriteLine("{0} pre-an updated (record ID: {1})", result, patientIdPKey);

                await App.PatientViewModel?.UpdateCharts(chart);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update pre-an {0}. Error: {1}", patientIdPKey, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update pre-an {0}. Error: {1}", patientIdPKey, ex.Message);
            }
        }

        /*
         * Update a chart.
         */
        /*public async Task UpdateIntraAnaesthesia(
            int recordIdPKey,
            string anaesStartHours,
            string anaesStartMinutes,
            string anaesEndHours,
            string anaesEndMinutes,
            string procStartHours,
            string procStartMinutes,
            string procEndHours,
            string procEndMinutes,
            string position,
            string warming,
            string throatPackIn,
            string throatPackOut,
            string swabsIn,
            string swabsOut,
            string sharpsIn,
            string sharpsOut,
            string notes
            )
        {
            ProcedureDetails procedure = await conn.FindAsync<ProcedureDetails>(recordIdPKey);
            IntraAnaesthesia chart = await conn.Table<IntraAnaesthesia>().Where(i => i.RecordIdPKey == recordIdPKey).FirstOrDefaultAsync();

            chart.AnaesStartHours = anaesStartHours;
            chart.AnaesStartMinutes = anaesStartMinutes;
            chart.AnaesEndHours = anaesEndHours;
            chart.AnaesEndMinutes = anaesEndMinutes;
            chart.ProcedureStartHours = procStartHours;
            chart.ProcedureStartMinutes = procStartMinutes;
            chart.ProcedureEndHours = procEndHours;
            chart.ProcedureEndMinutes = procEndMinutes;
            chart.PatientPosition = position;
            chart.Warming = warming;
            chart.ThroatPackIn = throatPackIn;
            chart.ThroatPackOut = throatPackOut;
            chart.SwabsIn = swabsIn;
            chart.SwabsOut = swabsOut;
            chart.SharpsIn = sharpsIn;
            chart.SharpsOut = sharpsOut;
            chart.Notes = notes;

            int result = 0;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update intra-anaesthesia details");

                result = await conn.UpdateAsync(chart);

                StatusMessage = string.Format("{0} intra-an updated (record ID: {1})", result, recordIdPKey);

                System.Diagnostics.Debug.WriteLine("{0} intra-an updated (record ID: {1})", result, recordIdPKey);

                await App.PatientViewModel?.UpdateCharts(procedure);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update intra-an {0}. Error: {1}", recordIdPKey, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update intra-an {0}. Error: {1}", recordIdPKey, ex.Message);
            }
        }*/

        /*
         * Update a chart.
         */
        /*public async Task UpdatePostAnaesthesia(
            int recordIdPKey,
            string timeExtubatedHours,
            string timeExtubatedMinutes,
            string ivCathRemoved,
            string ivCathFlushed,
            string recoverInstruct,
            string painPlan,
            string medsTGH
            )
        {
            ProcedureDetails procedure = await conn.FindAsync<ProcedureDetails>(recordIdPKey);
            PostAnaesthesia chart = await conn.Table<PostAnaesthesia>().Where(i => i.RecordIdPKey == recordIdPKey).FirstOrDefaultAsync();

            chart.TimeExtubatedHours = timeExtubatedHours;
            chart.TimeExtubatedMinutes = timeExtubatedMinutes;
            chart.IvCathRemoved = ivCathRemoved;
            chart.IvCathFlushed = ivCathFlushed;
            chart.RecoverInstruct = recoverInstruct;
            chart.PainPlan = painPlan;
            chart.MedsTGH = medsTGH;

            int result = 0;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update post-anaesthesia details");

                result = await conn.UpdateAsync(chart);

                StatusMessage = string.Format("{0} post-an updated (record ID: {1})", result, recordIdPKey);

                System.Diagnostics.Debug.WriteLine("{0} post-an updated (record ID: {1})", result, recordIdPKey);

                await App.PatientViewModel?.UpdateCharts(procedure);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update post-an {0}. Error: {1}", recordIdPKey, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update post-an {0}. Error: {1}", recordIdPKey, ex.Message);
            }
        }*/

        /*
         * Update a chart.
         */
        /*public async Task UpdateDrugsFluids(
            int recordIdPKey,
            int drugId,
            int fluidId,
            int inhalant,
            int flowRate
            )
        {
            ProcedureDetails procedure = await conn.FindAsync<ProcedureDetails>(recordIdPKey);
            DrugsFluids chart = await conn.Table<DrugsFluids>().Where(i => i.RecordIdPKey == recordIdPKey).FirstOrDefaultAsync();

            chart.DrugId = drugId;
            chart.FluidId = fluidId;
            chart.Inhalant = inhalant;
            chart.FlowRate = flowRate;

            int result = 0;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update DrugsFluids details");

                result = await conn.UpdateAsync(chart);

                StatusMessage = string.Format("{0} DrugsFluids updated (record ID: {1})", result, recordIdPKey);

                System.Diagnostics.Debug.WriteLine("{0} DrugsFluids updated (record ID: {1})", result, recordIdPKey);

                await App.PatientViewModel?.UpdateCharts(procedure);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update DrugsFluids {0}. Error: {1}", recordIdPKey, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update DrugsFluids {0}. Error: {1}", recordIdPKey, ex.Message);
            }
        }*/

        /*
         * Update a chart.
         */
        /*public async Task UpdateParameters(
            int recordIdPKey,
            int temp,
            int weight,
            int heartRate,
            int respRate,
            string mucousMems,
            string capRefillTime,
            int painScore,
            string pulseQual,
            string comments,
            int spO2,
            int eTCO2,
            string jawTone,
            string palpebral,
            string eyePos,
            int systolicBP,
            int diastolicBP,
            int meanBP,
            int dopplerBP,
            string eyesLubed
            )
        {
            ProcedureDetails procedure = await conn.FindAsync<ProcedureDetails>(recordIdPKey);
            Parameters chart = await conn.Table<Parameters>().Where(i => i.RecordIdPKey == recordIdPKey).FirstOrDefaultAsync();

            chart.Temp = temp;
            chart.Weight = weight;
            chart.HeartRate = heartRate;
            chart.RespRate = respRate;
            chart.MucousMems = mucousMems;
            chart.CapRefilTime = capRefillTime;
            chart.PainScore = painScore;
            chart.PulseQual = pulseQual;
            chart.Comments = comments;
            chart.SpO2 = spO2;
            chart.ETCO2 = eTCO2;
            chart.JawTone = jawTone;
            chart.Palpebral = palpebral;
            chart.EyePos = eyePos;
            chart.SystolicBP = systolicBP;
            chart.DiastolicBP = diastolicBP;
            chart.MeanBP = meanBP;
            chart.DopplerBP = dopplerBP;
            chart.EyesLubed = eyesLubed;

            int result = 0;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update SelectedParams details");

                result = await conn.UpdateAsync(chart);

                StatusMessage = string.Format("{0} SelectedParams updated (record ID: {1})", result, recordIdPKey);

                System.Diagnostics.Debug.WriteLine("{0} SelectedParams updated (record ID: {1})", result, recordIdPKey);

                await App.PatientViewModel?.UpdateCharts(procedure);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update SelectedParams {0}. Error: {1}", recordIdPKey, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update SelectedParams {0}. Error: {1}", recordIdPKey, ex.Message);
            }
        }*/

        /*
         * Update the currently selected Patient.
         */
        public async Task UpdatePatient(
            int id,
            string clientId,
            string clientName,
            string phoneNumber,
            string patientId,
            string patientName,
            string species,
            string age,
            string sex,
            string breed,
            string neuteredStatus,
            string temperament,
            double weight
            )
        {
            Patient thisPatient = new()
            {
                Id = id,
                ClientId = clientId,
                ClientName = clientName,
                PhoneNumber = phoneNumber,
                PatientId = patientId,
                PatientName = patientName,
                Species = species,
                Age = age,
                Sex = sex,
                Breed = breed,
                NeuteredStatus = neuteredStatus,
                Temperament = temperament,
                Weight = weight
            };

            await conn.UpdateAsync(thisPatient);

            await App.PatientViewModel?.UpdatePatients(thisPatient);
        }

        /*
         * Get a list of all patients in the Patient table.
         */
        public async Task<List<Patient>> GetAllPatients()
        {
            try
            {
                await Init(); // verify database initialisation
                return await conn.Table<Patient>().ToListAsync(); // create a list of all patients in the Patient table
            }
            catch (Exception ex) // if something goes wrong
            {
                // alert the user if it didn't work and display an error message.
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Patient>(); // return the list of patients
        }

        /*
         * Get a list of all charts in the Chart table.
         */
        public async Task<List<Chart>> GetAllCharts()
        {
            try
            {
                await Init(); // verify database initialisation
                return await conn.Table<Chart>().ToListAsync(); // create a list of all charts in the Chart table
            }
            catch (Exception ex) // if something goes wrong
            {
                // alert the user if it didn't work and display an error message.
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Chart>(); // return the list of charts
        }

        /*
         * Return a list of the anaesthesia charts belonging to a specific patient by chart ID.
         */
        public async Task<List<Chart>> GetChartsForPatient(PatientVM patient)
        {
            try
            {
                await Init();
                System.Diagnostics.Debug.WriteLine("Retrieving charts for Patient with ID: {0}", patient.Id);
                return await conn.Table<Chart>().Where(i => i.PatientIdPKey == patient.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Chart>();
        }

        /*
         * Return a list of the DrugsFluids sections belonging to a specific chart by chart ID.
         */
        /*public async Task<List<DrugsFluids>> GetDrugsFluidsForChart(ProcedureDetailsVM chart)
        {
            try
            {
                await Init();
                System.Diagnostics.Debug.WriteLine("Retrieving DrugsFluids for Procedure with ID: {0}", chart.Id);
                return await conn.Table<DrugsFluids>().Where(i => i.RecordIdPKey == chart.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<DrugsFluids>();
        }*/

        /*
         * Return a list of the SelectedParams sections belonging to a specific chart by chart ID.
         */
        /*public async Task<List<Parameters>> GetParametersForChart(ProcedureDetailsVM chart)
        {
            try
            {
                await Init();
                System.Diagnostics.Debug.WriteLine("Retrieving SelectedParams for Procedure with ID: {0}", chart.Id);
                return await conn.Table<Parameters>().Where(i => i.RecordIdPKey == chart.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Parameters>();
        }*/

        /*
         * Get a specific Patient by ID.
         */
        public async Task<Patient> GetPatient(int id)
        {
            Patient patient = new();

            try
            {
                await Init();

                patient = await conn.FindAsync<Patient>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return patient;
        }

        /*
         * Get Chart by ID.
         */
        public async Task<Chart> GetProcedureDetails(int id)
        {
            Chart chart = new();

            try
            {
                await Init();

                chart = await conn.FindAsync<Chart>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return chart;
        }

        /*
         * Get PreAnaesthesia by ID.
         */
        /*public async Task<PreAnaesthesia> GetPreAnaesthesia(int id)
        {
            PreAnaesthesia preAnaesthesia = new();

            try
            {
                await Init();

                preAnaesthesia = await conn.FindAsync<PreAnaesthesia>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return preAnaesthesia;
        }*/

        /*
         * Get IntraAnaesthesia by ID.
         */
        /*public async Task<IntraAnaesthesia> GetIntraAnaesthesia(int id)
        {
            IntraAnaesthesia intraAnaesthesia = new();

            try
            {
                await Init();

                intraAnaesthesia = await conn.FindAsync<IntraAnaesthesia>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return intraAnaesthesia;
        }*/

        /*
         * Get PostAnaesthesia by ID.
         */
        /*public async Task<PostAnaesthesia> GetPostAnaesthesia(int id)
        {
            PostAnaesthesia postAnaesthesia = new();

            try
            {
                await Init();

                postAnaesthesia = await conn.FindAsync<PostAnaesthesia>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return postAnaesthesia;
        }*/

        /*
         * Get SelectedParams by ID.
         */
        /*public async Task<Parameters> GetParameters(int id)
        {
            Parameters parameters = new();

            try
            {
                await Init();

                parameters = await conn.FindAsync<Parameters>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return parameters;
        }*/

        /*
         * Get the PreAnaesthesia VM by RecordIdPKey.
         */
        /*public async Task<PreAnaesthesiaVM> GetPreAnaesthesiaVM(int id)
        {
            await Init();

            PreAnaesthesia chart = await conn.Table<PreAnaesthesia>().FirstOrDefaultAsync(c => c.RecordIdPKey == id);
            PreAnaesthesiaVM vM = new(chart);

            return vM;
        }*/

        /*
         * Get the IntraAnaesthesia VM by RecordIdPKey.
         */
        /*public async Task<IntraAnaesthesiaVM> GetIntraAnaesthesiaVM(int id)
        {
            await Init();

            IntraAnaesthesia chart = await conn.Table<IntraAnaesthesia>().FirstOrDefaultAsync(c => c.RecordIdPKey == id);
            IntraAnaesthesiaVM vM = new(chart);

            return vM;
        }*/

        /*
         * Get the PostAnaesthesia VM by RecordIdPKey.
         */
        /*public async Task<PostAnaesthesiaVM> GetPostAnaesthesiaVM(int id)
        {
            await Init();

            PostAnaesthesia chart = await conn.Table<PostAnaesthesia>().FirstOrDefaultAsync(c => c.RecordIdPKey == id);
            PostAnaesthesiaVM vM = new(chart);

            return vM;
        }*/

        /*
         * Delete a Patient.
         */
        public async Task DeletePatient(PatientVM patientVM)
        {
            int result = 0;
            try
            {
                await Init();

                App.PatientViewModel?.ForceSelectPatient(patientVM);

                Patient patient = await conn.Table<Patient>().Where(i => i.Id == patientVM.Id).FirstOrDefaultAsync();
                string name = patient.PatientName;
                await App.PatientViewModel?.RefreshCharts();

                foreach (ChartVM chart in App.PatientViewModel?.LastPatientSelected.Charts)
                {
                    await DeleteChart(chart);
                }

                App.PatientViewModel?.Patients.Remove(patientVM);
                result = await conn.DeleteAsync(patient);
                await App.PatientViewModel?.RefreshPatients();

                StatusMessage = string.Format("{0} Patient(s) deleted (name: {1})", result, name);
                System.Diagnostics.Debug.WriteLine("{0} Patient(s) deleted (Patient name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not delete {0}. Error: {1}", patientVM.Id, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not delete {0}. Error: {1}", patientVM.Id, ex.Message);
            }

            //await App.ClientViewModel?.RefreshPatients();
        }

        /*
         * Delete an anaesthesia chart.
         */
        public async Task DeleteChart(ChartVM chartVM)
        {
            string procedure = chartVM.Procedure;

            int result = 0;
            try
            {
                await Init();

                App.PatientViewModel?.ForceSelectChart(chartVM);

                Chart thisChart = await conn.Table<Chart>().Where(i => i.Id == chartVM.Id).FirstOrDefaultAsync();

                App.PatientViewModel?.LastPatientSelected.Charts.Remove(chartVM);
                result = await conn.DeleteAsync(thisChart);

                StatusMessage = string.Format("{0} procedure(s) deleted (_procedure: {1})", result, procedure);
                System.Diagnostics.Debug.WriteLine("{0} procedure(s) deleted (procedure: {1})", result, procedure);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not delete procedure {0}. Error: {1}", chartVM.Id, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not delete procedure {0}. Error: {1}", chartVM.Id, ex.Message);
            }

            await App.PatientViewModel?.RefreshCharts();
        }
    }
}
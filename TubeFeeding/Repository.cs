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
                Food food = await conn.FindAsync<Food>(App.SchedulePages?.SelectedFood.Id);
                Schedule schedule = await conn.FindAsync<Schedule>(App.SchedulePages?.SelectedSchedule.Id);

                int scheduleId = schedule.Id;

                FeedingSchedule feedingSchedule = new FeedingSchedule
                {
                    Food = food,
                    Schedule = schedule
                };

                ExportDoc output = new ExportDoc(feedingSchedule);
                string pdf = Globals.GetLocalPath($"{schedule.PatientName}_{schedule.ClientName}_{food.Name}.pdf");
                output.GeneratePdf(pdf);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = $"{schedule.PatientName} {schedule.ClientName} - Tube Feeding Schedule",
                    File = new ShareFile(pdf)
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }
        }

        /*
         * FOR DEBUGGING ONLY - Drop the current Food table.
         */
        public async Task DropFoodTable()
        {
            System.Diagnostics.Debug.WriteLine("Attempting to drop Food table");

            try
            {
                await conn.DropTableAsync<Food>();
                System.Diagnostics.Debug.WriteLine("Dropping Food table");
                await conn.CreateTableAsync<Food>(); // create a table for storing Food data
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }

            await DropScheduleTable();
        }

        /*
         * FOR DEBUGGING ONLY - Drop the current Schedule table.
         */
        public async Task DropScheduleTable()
        {
            System.Diagnostics.Debug.WriteLine("Attempting to drop Schedule table");

            try
            {
                await conn.DropTableAsync<Schedule>();
                System.Diagnostics.Debug.WriteLine("Dropping Schedule table");
                await conn.CreateTableAsync<Schedule>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not drop table. Error: {1}", ex.Message);
            }
        }

        /*
         * Add a new Food to the Food table.
         */
        public async Task AddNewFood(
            string name,
            double kcal,
            double kcalPerGram,
            double netWeight,
            double dryWeight,
            double waterContent
            )
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add Food");

            Food food = new()
            {
                Name = name,
                Kcal = kcal,
                KcalPerGram = kcalPerGram,
                NetWeight = netWeight,
                DryWeight = dryWeight,
                WaterContent = waterContent
            };

            int result;

            try
            {
                await Init();

                result = await conn.InsertAsync(food);

                StatusMessage = string.Format("{0} Food added (name: {1})", result, name);
                System.Diagnostics.Debug.WriteLine("{0} Food added (name: {1})", result, name);

                await App.SchedulePages?.UpdateFoods(food);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add Food {0}. Error: {1}", name, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not add Food {0}. Error: {1}", name, ex.Message);
            }
        }

        /*
         * Add a new schedule to the Schedule table.
         */
        public async Task AddNewSchedule(
            int foodIdPKey,
            string foodName,
            string patientName,
            string clientName,
            string species,
            double bodyWeight,
            double rER,
            double fluidsPerDayTotal,
            double maxTotalVolumePerMeal,
            double foodPerDay,
            double foodPerMeal,
            double waterPerDay,
            double waterPerMeal,
            int mealsPerDay
            )
        {
            System.Diagnostics.Debug.WriteLine("Attempting to add chart");

            Schedule schedule = new()
            {
                FoodIdPKey = foodIdPKey,
                FoodName = foodName,
                PatientName = patientName,
                ClientName = clientName,
                Species = species,
                BodyWeight = bodyWeight,
                RER = rER,
                FluidsPerDayTotal = fluidsPerDayTotal,
                MaxTotalVolumePerMeal = maxTotalVolumePerMeal,
                FoodPerDay = foodPerDay,
                FoodPerMeal = foodPerMeal,
                WaterPerDay = waterPerDay,
                WaterPerMeal = waterPerMeal,
                MealsPerDay = mealsPerDay
            };

            int result;
            try
            {
                await Init();

                result = await conn.InsertAsync(schedule);

                StatusMessage = string.Format("{0} schedule added (record ID: {1})", result, schedule.Id);

                System.Diagnostics.Debug.WriteLine("{0} schedule added (record ID: {1})", result, schedule.Id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not add schedule record ID {0}. Error: {1}", schedule.Id, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not add schedule record ID {0}. Error: {1}", schedule.Id, ex.Message);
            }

            await App.SchedulePages?.UpdateSchedules(schedule);
        }

        /*
         * Update a Food.
         */
        public async Task UpdateFood(
            string name,
            double kcal,
            double kcalPerGram,
            double netWeight,
            double dryWeight,
            double waterContent
            )
        {
            Food food = new()
            {
                Name = name,
                Kcal = kcal,
                KcalPerGram = kcalPerGram,
                NetWeight = netWeight,
                DryWeight = dryWeight,
                WaterContent = waterContent
            };

            int result;
            try
            {
                await Init();

                System.Diagnostics.Debug.WriteLine("Attempting to update Food details");

                result = await conn.UpdateAsync(food);

                StatusMessage = string.Format("{0} Food updated (name: {1})", result, name);

                System.Diagnostics.Debug.WriteLine("{0} Food updated (name: {1})", result, name);

                await App.SchedulePages?.UpdateFoods(food);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not update Food {0}. Error: {1}", name, ex.Message);

                System.Diagnostics.Debug.WriteLine("Could not update Food {0}. Error: {1}", name, ex.Message);
            }
        }

        /*
         * Update the currently selected Schedule.
         */
        public async Task UpdateSchedule(
            int foodIdPKey,
            string foodName,
            string patientName,
            string clientName,
            string species,
            double bodyWeight,
            double rER,
            double fluidsPerDayTotal,
            double maxTotalVolumePerMeal,
            double foodPerDay,
            double foodPerMeal,
            double waterPerDay,
            double waterPerMeal,
            int mealsPerDay
            )
        {
            Schedule schedule = new()
            {
                FoodIdPKey = foodIdPKey,
                FoodName = foodName,
                PatientName = patientName,
                ClientName = clientName,
                Species = species,
                BodyWeight = bodyWeight,
                RER = rER,
                FluidsPerDayTotal = fluidsPerDayTotal,
                MaxTotalVolumePerMeal = maxTotalVolumePerMeal,
                FoodPerDay = foodPerDay,
                FoodPerMeal = foodPerMeal,
                WaterPerDay = waterPerDay,
                WaterPerMeal = waterPerMeal,
                MealsPerDay = mealsPerDay
            };

            await conn.UpdateAsync(schedule);

            await App.SchedulePages?.UpdateSchedules(schedule);
        }

        /*
         * Get a list of all foods.
         */
        public async Task<List<Food>> GetAllFoods()
        {
            try
            {
                await Init(); // verify database initialisation
                return await conn.Table<Food>().ToListAsync(); // create a list of all foods in the Food table
            }
            catch (Exception ex) // if something goes wrong
            {
                // alert the user if it didn't work and display an error message.
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Food>(); // return the list of foods
        }

        /*
         * Get a list of all schedules.
         */
        public async Task<List<Schedule>> GetAllSchedules()
        {
            try
            {
                await Init();
                return await conn.Table<Schedule>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Schedule>();
        }

        /*
         * Return a list of the schedules associated with a specific food.
         */
        public async Task<List<Schedule>> GetChartsForPatient(FoodPageModel food)
        {
            try
            {
                await Init();
                System.Diagnostics.Debug.WriteLine("Retrieving schedules for Food with ID: {0}", food.Id);
                return await conn.Table<Schedule>().Where(i => i.FoodIdPKey == food.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return new List<Schedule>();
        }

        /*
         * Get a specific Food by ID.
         */
        public async Task<Food> GetPatient(int id)
        {
            Food food = new();

            try
            {
                await Init();

                food = await conn.FindAsync<Food>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return food;
        }

        /*
         * Get Schedule by ID.
         */
        public async Task<Schedule> GetProcedureDetails(int id)
        {
            Schedule schedule = new();

            try
            {
                await Init();

                schedule = await conn.FindAsync<Schedule>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Data retrieval failed. {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("Data retrieval failed. {0}", ex.Message);
            }

            return schedule;
        }

        /*
         * Delete a Food.
         */
        public async Task DeleteFood(FoodPageModel foodPageModel)
        {
            int result = 0;
            try
            {
                await Init();

                App.SchedulePages?.ForceSelectFood(foodPageModel);

                Food food = await conn.Table<Food>().Where(i => i.Id == foodPageModel.Id).FirstOrDefaultAsync();
                string name = food.Name;
                await App.SchedulePages?.RefreshSchedules();

                foreach (SchedulePageModel schedule in App.SchedulePages?.Schedules)
                {
                    await DeleteChart(schedule);
                }

                App.SchedulePages?.Foods.Remove(foodPageModel);
                result = await conn.DeleteAsync(food);
                await App.SchedulePages?.RefreshFoods();

                StatusMessage = string.Format("{0} Patient(s) deleted (name: {1})", result, name);
                System.Diagnostics.Debug.WriteLine("{0} Patient(s) deleted (Patient name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not delete {0}. Error: {1}", foodPageModel.Id, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not delete {0}. Error: {1}", foodPageModel.Id, ex.Message);
            }
        }

        /*
         * Delete a schedule.
         */
        public async Task DeleteChart(SchedulePageModel schedulePageModel)
        {
            string name = schedulePageModel.PatientName + " " + schedulePageModel.ClientName;

            int result = 0;
            try
            {
                await Init();

                App.SchedulePages?.ForceSelectSchedule(schedulePageModel);

                Schedule thisSchedule = await conn.Table<Schedule>().Where(i => i.Id == schedulePageModel.Id).FirstOrDefaultAsync();

                App.SchedulePages?.Schedules.Remove(schedulePageModel);
                result = await conn.DeleteAsync(thisSchedule);

                StatusMessage = string.Format("{0} schedule(s) deleted (schedule: {1})", result, name);
                System.Diagnostics.Debug.WriteLine("{0} schedule(s) deleted (schedule: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Could not delete schedule {0}. Error: {1}", schedulePageModel.Id, ex.Message);
                System.Diagnostics.Debug.WriteLine("Could not delete schedule {0}. Error: {1}", schedulePageModel.Id, ex.Message);
            }

            await App.SchedulePages?.RefreshSchedules();
        }
    }
}
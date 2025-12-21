using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class ScheduleListPageModel : ObservableObject
    {
        public ObservableCollection<SchedulePageModel> Schedules { get; set; }
        public ObservableCollection<FoodPageModel> Foods { get; set; }
        public SchedulePageModel LastScheduleSelected { get; set; }
        public FoodPageModel LastFoodSelected { get; set; }

        public ScheduleListPageModel()
        {
            Schedules = [];
            Foods = [];
        }

        private SchedulePageModel? _selectedSchedule;

        public SchedulePageModel? SelectedSchedule
        {
            get => _selectedSchedule;
            set => SetProperty(ref _selectedSchedule, value);
        }

        private FoodPageModel? _selectedFood;

        public FoodPageModel? SelectedFood
        {
            get => _selectedFood;
            set => SetProperty(ref _selectedFood, value);
        }

        /*
         * Force Food selection.
         */
        public void ForceSelectFood(FoodPageModel food)
        {
            SelectedFood = food;
            LastFoodSelected = food;
        }

        /*
         * Force chart selection.
         */
        public void ForceSelectSchedule(SchedulePageModel schedule)
        {
            SelectedSchedule = schedule;
        }

        /*
         * Update the list of schedules and the selected schedule.
         */
        public async Task UpdateSchedules(Schedule selectedSchedule)
        {
            IEnumerable<Schedule> schedulesData = await App.Repo.GetAllSchedules();
            Schedules = [];

            foreach (Schedule schedule in schedulesData)
            {
                Schedules.Add(new SchedulePageModel(schedule));
            }

            foreach (SchedulePageModel schedule in Schedules)
            {
                if (schedule.Id == selectedSchedule.Id)
                {
                    ForceSelectSchedule(schedule);
                    System.Diagnostics.Debug.WriteLine($"Selected {SelectedSchedule.PatientName} {SelectedSchedule.ClientName} (ScheduleListPageModel)");
                    break;
                }
            }

            if (SelectedSchedule != null)
            {
                await SelectedSchedule.CalculateSchedule();
            }
        }

        /*
         * Update the list of foods and the selected food.
         */
        public async Task UpdateFoods(Food selectedFood)
        {
            IEnumerable<Food> foodsData = await App.Repo.GetAllFoods();
            Foods = [];

            foreach (Food food in foodsData)
            {
                Foods.Add(new FoodPageModel(food));
            }

            foreach (FoodPageModel food in Foods)
            {
                if (food.Id == selectedFood.Id)
                {
                    ForceSelectFood(food);
                    System.Diagnostics.Debug.WriteLine($"Selected {SelectedFood.Name} (ScheduleListPageModel)");
                    break;
                }
            }
        }

        /*
         * Refresh the visible list of schedules when data is changed.
         */
        public async Task RefreshSchedules()
        {
            IEnumerable<Schedule> schedulesData = await App.Repo.GetAllSchedules();
            Schedules.Clear();

            foreach (Schedule schedule in schedulesData)
            {
                Schedules.Add(new SchedulePageModel(schedule));
            }

            System.Diagnostics.Debug.WriteLine("Schedule list refreshed (ScheduleListPageModel)");
        }

        /*
         * Refresh the visible list of foods when data is changed.
         */
        public async Task RefreshFoods()
        {
            IEnumerable<Food> foodsData = await App.Repo.GetAllFoods();
            Foods.Clear();

            foreach (Food food in foodsData)
            {
                Foods.Add(new FoodPageModel(food));
            }

            System.Diagnostics.Debug.WriteLine("Food list refreshed (ScheduleListPageModel)");
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class PatientListPageModel : ObservableObject
    {
        public ObservableCollection<PatientPageModel> Patients { get; set; }
        public ObservableCollection<FoodPageModel> Foods { get; set; }
        public PatientPageModel LastPatientSelected { get; set; }
        public FoodPageModel LastFoodSelected { get; set; }

        public PatientListPageModel()
        {
            Patients = [];
            Foods = [];
        }

        private PatientPageModel? _selectedPatient;

        public PatientPageModel? SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
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
        public void ForceSelectSchedule(PatientPageModel schedule)
        {
            SelectedPatient = schedule;
        }

        /*
         * Update the list of schedules and the selected schedule.
         */
        public async Task UpdateSchedules(Patient selectedPatient)
        {
            IEnumerable<Patient> patientsData = await App.Repo.GetAllPatients();
            Patients = [];

            foreach (Patient patient in patientsData)
            {
                Patients.Add(new PatientPageModel(patient));
            }

            foreach (PatientPageModel patient in Patients)
            {
                if (patient.Id == selectedPatient.Id)
                {
                    ForceSelectSchedule(patient);
                    System.Diagnostics.Debug.WriteLine($"Selected {SelectedPatient.PatientName} {SelectedPatient.ClientName} (PatientListPageModel)");
                    break;
                }
            }

            if (SelectedPatient != null)
            {
                await SelectedPatient.CalculatePatient();
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
                    System.Diagnostics.Debug.WriteLine($"Selected {SelectedFood.Name} (PatientListPageModel)");
                    break;
                }
            }
        }

        /*
         * Refresh the visible list of schedules when data is changed.
         */
        public async Task RefreshPatients()
        {
            IEnumerable<Patient> patientData = await App.Repo.GetAllPatients();
            Patients.Clear();

            foreach (Patient patient in patientData)
            {
                Patients.Add(new PatientPageModel(patient));
            }

            System.Diagnostics.Debug.WriteLine("Patient list refreshed (PatientListPageModel)");
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

            System.Diagnostics.Debug.WriteLine("Food list refreshed (PatientListPageModel)");
        }
    }
}
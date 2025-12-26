using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class PatientListPageModel : ObservableObject
    {
        public ObservableCollection<PatientPageModel> Patients { get; set; }
        public PatientPageModel LastPatientSelected { get; set; }

        public PatientListPageModel()
        {
            Patients = [];
        }

        private PatientPageModel? _selectedPatient;

        public PatientPageModel? SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        /*
         * Force patient selection.
         */
        public void ForceSelectPatient(PatientPageModel patient)
        {
            SelectedPatient = patient;
            LastPatientSelected = patient;
        }

        /*
         * Update the list of schedules and the selected schedule.
         */
        public async Task UpdatePatients(Patient selectedPatient)
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
                    ForceSelectPatient(patient);
                    System.Diagnostics.Debug.WriteLine($"Selected {SelectedPatient.PatientName} {SelectedPatient.ClientName} (PatientListPageModel)");
                    break;
                }
            }

            /*if (SelectedPatient != null)
            {
                await SelectedPatient.ComposeSchedule();
            }*/
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
    }
}
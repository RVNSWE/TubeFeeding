namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.SchedulePages?.SelectedPatient;

        InitializeComponent();

        btnCreatePDF.Clicked += async (s, e) => await CreatePDF();

        btnDeleteSchedule.Clicked += async (s, e) => await DeletePatient();
    }

    /*
     * Run before the page appears.
     */
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // If navigating back to patient page after selecting a chart, re-select patient.
        if (App.SchedulePages?.SelectedPatient == null && App.SchedulePages?.LastPatientSelected != null)
        {
            App.SchedulePages?.ForceSelectPatient(App.SchedulePages.LastPatientSelected);
        }
        else
        {
            App.SchedulePages.LastPatientSelected = App.SchedulePages.SelectedPatient;
        }

        if (App.SchedulePages?.SelectedPatient != null)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Selected {App.SchedulePages?.SelectedPatient.PatientName} {App.SchedulePages.SelectedPatient.ClientName} (PatientDetailsPage)"
                );
        }

        Dispatcher.DispatchAsync(App.SchedulePages.RefreshPatients);
    }

    /*
     * Export this chart to PDF.
     */
    public async Task CreatePDF()
    {
        int patientId;

        if (App.SchedulePages?.SelectedPatient == null)
        {
            patientId = App.SchedulePages.LastPatientSelected.Id;
        }
        else
        {
            patientId = App.SchedulePages.SelectedPatient.Id;
        }

        System.Diagnostics.Debug.WriteLine($"Creating PDF for patient ID {patientId}");

        await App.Repo.OutputChart(patientId);
    }

    /*
     * Delete this Patient.
     */
    public static async Task DeletePatient()
    {
        PatientPageModel lastPatientSelected = App.SchedulePages?.LastPatientSelected;
        PatientPageModel selectedPatient = App.SchedulePages?.SelectedPatient;

        if (selectedPatient == null && lastPatientSelected != null)
        {
            await App.Repo.DeletePatient(lastPatientSelected);
        }
        else
        {
            await App.Repo.DeletePatient(selectedPatient);
        }

        Globals.GoToList();
    }

    /*
     * Override the back button.
     */
    protected override bool OnBackButtonPressed()
    {
        Globals.GoToList();
        return true;
    }
}
namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
	public PatientDetailsPage()
    {
        BindingContext = App.SchedulePages?.SelectedPatient;

        InitializeComponent();

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
     * Delete this Patient.
     */
    public static async Task DeletePatient()
    {
        PatientPageModel patientPageModel = App.SchedulePages?.SelectedPatient;

        await App.Repo.DeletePatient(patientPageModel);

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
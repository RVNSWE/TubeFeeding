namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

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
        if (App.PatientPage?.SelectedPatient == null && App.PatientPage?.LastPatientSelected != null)
        {
            App.PatientPage?.ForceSelectPatient(App.PatientPage.LastPatientSelected);
        }
        else
        {
            App.PatientPage.LastPatientSelected = App.PatientPage.SelectedPatient;
        }

        if (App.PatientPage?.SelectedPatient != null)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Selected {App.PatientPage?.SelectedPatient.PatientName} {App.PatientPage.SelectedPatient.ClientName} (PatientDetailsPage)"
                );
        }

        Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
    }

    /*
     * Export this chart to PDF.
     */
    public async Task CreatePDF()
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

        System.Diagnostics.Debug.WriteLine($"Creating PDF for patient ID {patientId}");

        await App.Repo.OutputChart(patientId);
    }

    /*
     * Delete this Patient.
     */
    public static async Task DeletePatient()
    {
        PatientPageModel lastPatientSelected = App.PatientPage?.LastPatientSelected;
        PatientPageModel selectedPatient = App.PatientPage?.SelectedPatient;

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
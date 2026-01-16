using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        btnCreatePDF.Clicked += async (s, e) => await CreatePdf();
        btnDeleteSchedule.Clicked += async (s, e) => await DeletePatient();
    }

    /*
     * Run before the page appears.
     */
    protected override void OnAppearing()
    {
        base.OnAppearing();

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
            App.PatientPage.LastPatientSelected = App.PatientPage.SelectedPatient;
            System.Diagnostics.Debug.WriteLine(
                $"Selected {App.PatientPage?.SelectedPatient.PatientName} {App.PatientPage.SelectedPatient.ClientName} (PatientDetailsPage)"
                );
        }

        Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
    }

    /*
     * Generate the PDF.
     */
    public static async Task CreatePdf()
    {
        App.PatientPage?.ForceSelectPatient(App.PatientPage.LastPatientSelected);
        PatientPageModel selectedPatient = App.PatientPage?.SelectedPatient;

        selectedPatient.GeneratingPdf = "Generating PDF, please wait...";
        System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient ID {selectedPatient.Id}");

        //await selectedPatient.GeneratePdf();
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
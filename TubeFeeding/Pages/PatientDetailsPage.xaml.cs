using TubeFeeding.Models;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        btnCreatePDF.Clicked += async (s, e) => await App.PatientPage?.SelectedPatient.CreatePDF();

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
            System.Diagnostics.Debug.WriteLine(
                $"Selected {App.PatientPage?.SelectedPatient.PatientName} {App.PatientPage.SelectedPatient.ClientName} (PatientDetailsPage)"
                );
        }

        Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
    }

    public async Task CreatePDF()
    {
        PatientPageModel selectedPatient = App.PatientPage?.SelectedPatient;

        selectedPatient.GeneratingPdf = "Generating PDF, please wait...";
        System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient ID {selectedPatient.Id}");

        try
        {
            Patient patient = await App.Repo.GetPatient(selectedPatient.Id);

            string pdfPath = Globals.GetLocalPath($"{patient.PatientName}_{patient.ClientName}_{patient.FoodName}.pdf");
            FeedingSchedule feedingSchedule = new(patient);
            ExportDoc output = new(feedingSchedule, pdfPath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = $"{patient.PatientName} {patient.ClientName} - Tube Feeding Plan",
                File = new ShareFile(pdfPath)
            });

            selectedPatient.GeneratingPdf = "PDF creation successful.";
            System.Diagnostics.Debug.WriteLine("PDF creation successful.");
        }
        catch (Exception ex)
        {
            selectedPatient.GeneratingPdf = "PDF generation failed. Error: " + ex.Message;
            System.Diagnostics.Debug.WriteLine("PDF generation failed. Error: " + ex.Message);
        }
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
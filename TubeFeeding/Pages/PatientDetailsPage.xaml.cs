using CommunityToolkit.Maui.Storage;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        btnGeneratePdf.Clicked += async (s, e) => await OnGeneratePdfButtonPressed();
        btnDeleteSchedule.Clicked += async (s, e) => await OnDeleteScheduleButtonPressed();
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
            App.PatientPage?.LastPatientSelected = App.PatientPage?.SelectedPatient;
        }

        if (App.PatientPage?.SelectedPatient != null)
        {
            App.PatientPage.LastPatientSelected = App.PatientPage.SelectedPatient;
            System.Diagnostics.Debug.WriteLine(
                $"Selected {App.PatientPage.SelectedPatient.PatientName} {App.PatientPage.SelectedPatient.ClientName} (PatientDetailsPage)"
                );
        }

        Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
    }

    public static async Task OnGeneratePdfButtonPressed()
    {
        PatientPageModel selectedPatient = App.PatientPage?.LastPatientSelected;

#if WINDOWS
        selectedPatient.GeneratingPdf = "Please select a save location";
        System.Diagnostics.Debug.WriteLine($"Please select a save location");

        CancellationTokenSource source = new();
        CancellationToken token = source.Token;
        var result = await FolderPicker.Default.PickAsync(token);

        selectedPatient.GeneratingPdf = "Generating PDF, please wait...";
        System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient {selectedPatient.NameString}");
        
        if (result.IsSuccessful)
        {
            selectedPatient.FilePath = result.Folder.Path;
            await selectedPatient.GeneratePdf();
        }
        else
        {
            selectedPatient.GeneratingPdf = "";
            System.Diagnostics.Debug.WriteLine($"PDF generation cancelled");
        }
#else
        selectedPatient.GeneratingPdf = "Generating PDF, please wait...";
        System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient {selectedPatient.NameString}");

        await selectedPatient.GeneratePdf();
#endif
    }

    public static async Task OnDeleteScheduleButtonPressed()
    {
        PatientPageModel lastPatientSelected = App.PatientPage?.LastPatientSelected;
        PatientPageModel selectedPatient = App.PatientPage?.SelectedPatient;

        if (selectedPatient == null && lastPatientSelected != null)
        {
            await App.Repo.DeleteSchedule(lastPatientSelected);
        }
        else
        {
            await App.Repo.DeleteSchedule(selectedPatient);
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
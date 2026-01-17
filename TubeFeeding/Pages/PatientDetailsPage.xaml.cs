using CommunityToolkit.Maui.Storage;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        btnGeneratePdf.Clicked += async (s, e) => await OnGeneratePdfButtonPressed(s, e);
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

    static async Task<bool> ArePermissionsGranted()
    {
        var readPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
        var writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

        if (readPermissionStatus is PermissionStatus.Granted
            && writePermissionStatus is PermissionStatus.Granted)
        {
            return true;
        }

        await Shell.Current.CurrentPage.DisplayAlertAsync("Storage permission is not granted.", "Please grant the permission to use this feature.", "OK");

        return false;
    }

    /*
     * Generate the PDF.
     */
    public static async Task OnGeneratePdfButtonPressed(object sender, EventArgs e)
    {
        if (!await ArePermissionsGranted())
        {
            return;
        }

        PatientPageModel selectedPatient = App.PatientPage?.LastPatientSelected;

        selectedPatient.GeneratingPdf = "Please select a save location";
        System.Diagnostics.Debug.WriteLine($"Please select a save location");

        CancellationTokenSource source = new();
        CancellationToken token = source.Token;
        var result = await FolderPicker.Default.PickAsync(token);

        selectedPatient.GeneratingPdf = "Generating PDF, please wait...";
        System.Diagnostics.Debug.WriteLine($"Attempting to create PDF for patient {selectedPatient.NameString}");

        if (result.IsSuccessful)
        {
            await selectedPatient.GeneratePdf(result.Folder.Path);
        }
        else
        {
            selectedPatient.GeneratingPdf = "";
            System.Diagnostics.Debug.WriteLine($"PDF generation cancelled");
        }
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
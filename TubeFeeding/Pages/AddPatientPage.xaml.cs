using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class AddPatientPage : ContentPage
{
    private readonly Label speciesLabel;

    public AddPatientPage()
    {
        InitializeComponent();

        App.PatientPage?.NewPatient = new AddPatientPageModel();
        BindingContext = App.PatientPage?.NewPatient;

        speciesLabel = new Label();
        speciesLabel.Text = "None";
        speciesLabel.SetBinding(Label.TextProperty, Binding.Create(static (Picker picker) => picker.SelectedItem, source: picker));

        btnSave.Clicked += async (s, e) => await App.PatientPage.NewPatient.SaveNewSchedule(
            newPatientName.Text,
            newClientName.Text,
            speciesLabel.Text,
            newBodyWeight.Text,
            newFoodName.Text,
            newKcal.Text,
            newNetWeight.Text,
            newWaterPercentage.Text
            );
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            speciesLabel.Text = (string)picker.ItemsSource[selectedIndex];
        }
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
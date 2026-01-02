using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class LandingPage : ContentPage
{
	public LandingPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
        //Dispatcher.DispatchAsync(App.SchedulePages.RefreshFoods);
        Globals.GoToList();
    }
}
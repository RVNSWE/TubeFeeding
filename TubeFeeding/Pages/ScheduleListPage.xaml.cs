namespace TubeFeeding.Pages;

public partial class ScheduleListPage : ContentPage
{
	public ScheduleListPage()
    {
        BindingContext = App.SchedulePages;

        InitializeComponent();

        btnCreateSchedule.Clicked += (s, e) => Globals.GoToAdd();
    }

    /*
     * Run before the page appears.
     */
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.DispatchAsync(App.SchedulePages.RefreshSchedules);
        Dispatcher.DispatchAsync(App.SchedulePages.RefreshFoods);
    }

    /*
     * Do when the selection changes.
     */
    public void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.SchedulePages.SelectedSchedule = e.CurrentSelection.FirstOrDefault() as SchedulePageModel;

        if (App.SchedulePages.SelectedSchedule != null)
        {
            Globals.GoToView();
        }
    }
}
namespace TubeFeeding.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = App.PatientPage;

            InitializeComponent();

            btnCreateSchedule.Clicked += (s, e) => Globals.GoToAdd();
        }

        /*
         * Run before the page appears.
         */
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Dispatcher.DispatchAsync(App.PatientPage.RefreshPatients);
            //Dispatcher.DispatchAsync(App.SchedulePages.RefreshFoods);
        }

        /*
         * Do when the selection changes.
         */
        public void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.PatientPage.SelectedPatient = e.CurrentSelection.FirstOrDefault() as PatientPageModel;

            if (App.PatientPage.SelectedPatient != null)
            {
                Globals.GoToView();
            }
        }
    }
}
namespace TubeFeeding
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            var currentTheme = Application.Current!.RequestedTheme;

            Routing.RegisterRoute("main", typeof(LandingPage));
            Routing.RegisterRoute("main/list", typeof(MainPage));
            Routing.RegisterRoute("main/view", typeof(PatientDetailsPage));
            Routing.RegisterRoute("main/add", typeof(AddPatientPage));
        }

        private void SfSegmentedControl_SelectionChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
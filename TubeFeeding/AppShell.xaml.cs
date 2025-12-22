namespace TubeFeeding
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("main", typeof(LandingPage));
            Routing.RegisterRoute("main/list", typeof(MainPage));
            Routing.RegisterRoute("main/view", typeof(PatientDetailsPage));
            Routing.RegisterRoute("main/add", typeof(AddPatientPage));
        }
    }
}
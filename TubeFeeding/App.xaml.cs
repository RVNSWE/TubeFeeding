namespace TubeFeeding
{
    public partial class App : Application
    {
        public static Repository Repo { get; private set; }
        public static PatientListPageModel? PatientPage { get; private set; }

        public App(Repository repo)
        {
            InitializeComponent();

            Repo = repo;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            PatientPage = new();
            PatientPage.RefreshPatients().ContinueWith((s) => { });

            return new Window(new AppShell());
        }
    }
}
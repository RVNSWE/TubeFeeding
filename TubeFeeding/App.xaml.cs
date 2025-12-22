namespace TubeFeeding
{
    public partial class App : Application
    {
        public static Repository Repo { get; private set; }
        public static PatientListPageModel? SchedulePages { get; private set; }

        public App(Repository repo)
        {
            InitializeComponent();

            Repo = repo;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            SchedulePages = new();
            SchedulePages.RefreshPatients().ContinueWith((s) => { });

            return new Window(new AppShell());
        }
    }
}
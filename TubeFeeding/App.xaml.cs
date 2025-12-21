using TubeFeeding.Clients;
using TubeFeeding.ViewModels;

namespace TubeFeeding
{
    public partial class App : Application
    {
        public static Repository Repo { get; private set; }
        public static ScheduleListPageModel? SchedulePages { get; private set; }

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            SchedulePages = new();
            SchedulePages.RefreshSchedules().ContinueWith((s) => { });

            return new Window(new AppShell());
        }
    }
}
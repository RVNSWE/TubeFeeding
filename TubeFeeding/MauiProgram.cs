using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using TubeFeeding.Pages.Controls;


namespace TubeFeeding
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            string dbPath = Globals.GetLocalPath(Globals.DatabaseName);
            builder.Services.AddSingleton<Repository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

            return builder.Build();
        }
    }
}
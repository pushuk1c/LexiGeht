
using LexiGeht.Data;
using LexiGeht.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;


namespace LexiGeht
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
                       
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-ExtraBold.ttf", "RobotoExtraBold");

                });

            
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "lexigeht.db3");
            builder.Services.AddPersistence(dbPath);
            

            // Customize MAUI Controls
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null; // прибирає підкреслення
#elif IOS
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            // Initialize the database
            app.Services.GetRequiredService<IDatabase>().InitializeAsync();

            return app;
        }
    }
}

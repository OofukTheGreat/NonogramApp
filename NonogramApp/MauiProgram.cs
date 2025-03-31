using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using NonogramApp.Services;
using NonogramApp.ViewModels;
using NonogramApp.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;

namespace NonogramApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Minecraft.ttf", "Font");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Continue initializing your .NET MAUI App here
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Minecraft.ttf", "Font");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDataServices()
                .RegisterPages()
                .RegisterViewModels();         

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignupPage>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<ApprovePuzzlesPage>();
            builder.Services.AddTransient<LevelSelectPage>();
            builder.Services.AddTransient<GamePage>();
            builder.Services.AddTransient<WelcomePage>();
            builder.Services.AddTransient<Leaderboard>();
            builder.Services.AddTransient<CreateLevelsPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<LogoutPage>();
            return builder;
        }

        public static MauiAppBuilder RegisterDataServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<NonogramService>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<SignupPageViewModel>();
            builder.Services.AddTransient<WelcomeViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<ApprovePuzzlesViewModel>();
            builder.Services.AddTransient<LevelSelectViewModel>();
            builder.Services.AddTransient<GameViewModel>();
            builder.Services.AddTransient<CreateLevelsViewModel>();
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<LogoutPageViewModel>();
            return builder;
        }
    }
}

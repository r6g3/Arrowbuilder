using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Arrowbuilder.Services;
using Arrowbuilder.ViewModels;
using Arrowbuilder.Views;

namespace Arrowbuilder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services registrieren
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AuthenticationService>();

        // ViewModels registrieren
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        // Views registrieren
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
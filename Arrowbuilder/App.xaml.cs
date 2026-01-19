using Microsoft.Extensions.DependencyInjection;
using Arrowbuilder.Services;
using Arrowbuilder.Views;

namespace Arrowbuilder;

public partial class App : Application
{
    private readonly AuthenticationService _authService;
    private readonly ApiService _apiService;

    public App(AuthenticationService authService, ApiService apiService)
    {
        InitializeComponent();

        _authService = authService;
        _apiService = apiService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += async (s, e) =>
        {
            await CheckAuthenticationAsync();
        };

        return window;
    }

    private async Task CheckAuthenticationAsync()
    {
        try
        {
            if (await _authService.IsAuthenticatedAsync())
            {
                var token = await _authService.GetTokenAsync();
                if (token != null)
                {
                    _apiService.SetAuthToken(token);
                    await Shell.Current.GoToAsync("///MainPage");
                }
                else
                {
                    await Shell.Current.GoToAsync("///LoginPage");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }
        catch
        {
            // Bei Fehler zur Login-Seite
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
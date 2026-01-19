using System.Windows.Input;
using Arrowbuilder.Services;
using Arrowbuilder.Models.DTOs;

namespace Arrowbuilder.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly AuthenticationService _authService;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(ApiService apiService, AuthenticationService authService)
        {
            _apiService = apiService;
            _authService = authService;

            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            NavigateToRegisterCommand = new Command(async () => await NavigateToRegister());
        }
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Bitte füllen Sie alle Felder aus";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var request = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var response = await _apiService.LoginAsync(request);

                if (response.Success && response.Data != null)
                {

                    //await _authService.SaveAuthDataAsync(AuthDataExtensions.ToAuthResponse(response));
                    _apiService.SetAuthToken(response.Data.Token);

                    await Shell.Current.GoToAsync("///MainPage");
                }
                else
                {
                    ErrorMessage = response.Message ?? "Login fehlgeschlagen";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}
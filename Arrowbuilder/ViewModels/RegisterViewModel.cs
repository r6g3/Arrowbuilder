using System.Windows.Input;
using Arrowbuilder.Services;
using Arrowbuilder.Models.DTOs;

namespace Arrowbuilder.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly AuthenticationService _authService;

        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _errorMessage = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
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

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel(ApiService apiService, AuthenticationService authService)
        {
            _apiService = apiService;
            _authService = authService;

            RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);
            NavigateToLoginCommand = new Command(async () => await NavigateToLogin());
        }

        private async Task RegisterAsync()
        {
            if (!ValidateInput())
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var request = new RegisterRequest
                {
                    Name = Name,
                    Email = Email,
                    Password = Password
                };

                var response = await _apiService.RegisterAsync(request);

                if (response.Success && response.Data != null)
                {
                    await _authService.SaveAuthDataAsync(response.Data);
                    _apiService.SetAuthToken(response.Data.Token);

                    // Navigation zur Hauptseite
                    await Shell.Current.GoToAsync("///MainPage");
                }
                else
                {
                    ErrorMessage = response.Message ?? "Registrierung fehlgeschlagen";
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

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Bitte geben Sie einen Namen ein";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Bitte geben Sie eine Email ein";
                return false;
            }

            if (!Email.Contains("@"))
            {
                ErrorMessage = "Bitte geben Sie eine gültige Email ein";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Bitte geben Sie ein Passwort ein";
                return false;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Passwort muss mindestens 6 Zeichen lang sein";
                return false;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwörter stimmen nicht überein";
                return false;
            }

            return true;
        }

        private async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
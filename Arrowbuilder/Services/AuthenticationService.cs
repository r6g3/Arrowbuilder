using System.Text.Json;
using Arrowbuilder.Models.DTOs;

namespace Arrowbuilder.Services
{
    public class AuthenticationService
    {
        private const string TokenKey = "auth_token";
        private const string UserKey = "user_data";

        // AuthResponse → AuthData
        public async Task SaveAuthDataAsync(AuthData authData)
        {
            await SecureStorage.SetAsync(TokenKey, authData.Token);
            var userData = JsonSerializer.Serialize(authData);
            await SecureStorage.SetAsync(UserKey, userData);
        }

        // AuthResponse → AuthData
        public async Task<AuthData?> GetAuthDataAsync()
        {
            try
            {
                var userData = await SecureStorage.GetAsync(UserKey);
                if (string.IsNullOrEmpty(userData))
                    return null;

                return JsonSerializer.Deserialize<AuthData>(userData);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authData = await GetAuthDataAsync();
            if (authData == null)
                return false;

            // Prüfen ob Token abgelaufen ist (mit 5 Min Puffer)
            return authData.ExpiresAt > DateTime.UtcNow.AddMinutes(5);
        }

        public async Task<bool> NeedsRefreshAsync()
        {
            var authData = await GetAuthDataAsync();
            if (authData == null)
                return false;

            // Token sollte erneuert werden wenn < 1 Stunde gültig
            var timeUntilExpiry = authData.ExpiresAt - DateTime.UtcNow;
            return timeUntilExpiry.TotalHours < 1;
        }

        public async Task LogoutAsync()
        {
            SecureStorage.Remove(TokenKey);
            SecureStorage.Remove(UserKey);
            SecureStorage.RemoveAll();

            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
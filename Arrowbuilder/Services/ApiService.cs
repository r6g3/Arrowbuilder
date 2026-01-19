using System.Net.Http.Json;
using System.Net.Http.Headers;
using Arrowbuilder.Models.DTOs;

namespace Arrowbuilder.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5032/api"; // Ihre API URL

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ApiResponse<AuthData>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/register", request);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<AuthData>>() 
                        ?? new ApiResponse<AuthData> { Success = false, Message = "Keine Antwort vom Server" };
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<AuthData> 
                { 
                    Success = false, 
                    Message = $"Fehler {response.StatusCode}: {errorContent}" 
                };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<AuthData> 
                { 
                    Success = false, 
                    Message = $"Netzwerkfehler: {ex.Message}" 
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthData> 
                { 
                    Success = false, 
                    Message = $"Fehler: {ex.Message}" 
                };
            }
        }

        public async Task<ApiResponse<AuthData>> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", request);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<AuthData>>() 
                        ?? new ApiResponse<AuthData> { Success = false, Message = "Keine Antwort vom Server" };
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<AuthData> 
                { 
                    Success = false, 
                    Message = $"Fehler {response.StatusCode}: {errorContent}" 
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthData> 
                { 
                    Success = false, 
                    Message = $"Fehler: {ex.Message}" 
                };
            }
        }
    }
}
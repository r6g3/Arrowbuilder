using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Arrowbuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validierung
                if (await _userRepository.UserExistsAsync(request.Email))
                {
                    return BadRequest(new ApiResponse<AuthData>
                    {
                        Success = false,
                        Message = "Email bereits registriert"
                    });
                }

                // Passwort hashen mit BCrypt oder PBKDF2
                var passwordHash = HashPassword(request.Password);

                // User erstellen
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateUserAsync(user);

                // Token generieren
                var token = _tokenService.GenerateToken(createdUser);

                var authData = new AuthData
                {
                    UserId = createdUser.Id,
                    Token = token,
                    Email = createdUser.Email,
                    Name = createdUser.Name,
                    ExpiresAt = DateTime.UtcNow.AddHours(24) // Token gültig für 24 Stunden
                };

                return Ok(new ApiResponse<AuthData>
                {
                    Success = true,
                    Data = authData,
                    Message = "Registrierung erfolgreich"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AuthData>
                {
                    Success = false,
                    Message = $"Serverfehler: {ex.Message}"
                });
            }
        }

        private string HashPassword(string password)
        {
            // Verwenden Sie BCrypt.Net-Next NuGet Package
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                
                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new ApiResponse<AuthData>
                    {
                        Success = false,
                        Message = "Ungültige Email oder Passwort"
                    });
                }

                var token = _tokenService.GenerateToken(user);

                var authData = new AuthData
                {
                    UserId = user.Id,
                    Token = token,
                    Email = user.Email,
                    Name = user.Name,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                return Ok(new ApiResponse<AuthData>
                {
                    Success = true,
                    Data = authData,
                    Message = "Login erfolgreich"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AuthData>
                {
                    Success = false,
                    Message = $"Serverfehler: {ex.Message}"
                });
            }
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
namespace Arrowbuilder.Models.DTOs
{
    public static class AuthDataExtensions
    {
        public static AuthResponse ToAuthResponse(this AuthData authData)
        {
            return new AuthResponse
            {
                UserId = authData.UserId,
                Token = authData.Token,
                Email = authData.Email,
                Name = authData.Name,
                ExpiresAt = authData.ExpiresAt
            };
        }
    }
}
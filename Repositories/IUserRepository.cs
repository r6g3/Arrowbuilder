namespace Arrowbuilder.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<User> CreateUserAsync(User user);

        
    
    }
}
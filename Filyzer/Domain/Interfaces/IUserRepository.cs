using Filyzer.Domain.Entities;

namespace Filyzer.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByApiKeyAsync(string apiKey);
        Task<User> GetByIdAsync(Guid id);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}

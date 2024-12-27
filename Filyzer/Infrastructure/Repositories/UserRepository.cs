using Filyzer.Domain.Entities;
using Filyzer.Domain.Interfaces;
using Filyzer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Filyzer.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FilyzerDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
                FilyzerDbContext context,
                ILogger<UserRepository> logger
            ) 
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> GetByApiKeyAsync(string apiKey)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.ApiKey == apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by API key");
                throw;
            }
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", id);
                throw;
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {UserId}", user.Id);
                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", user.Id);
                throw;
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                throw;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        public async Task<(IEnumerable<User> users, double totalCount)> GetPaginatedAsync(int page, int pageSize)
        {
            try
            {
                var query = _context.Users.AsNoTracking();

                var totalCount = await query.CountAsync();

                var users = await query
                    .OrderBy(u => u.Email)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (users, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated users");
                throw;
            }
        }
    }
}

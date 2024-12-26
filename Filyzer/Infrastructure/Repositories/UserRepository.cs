using Filyzer.Domain.Entities;
using Filyzer.Domain.Interfaces;
using Filyzer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Filyzer.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FilyzerDbContext _context;
        public UserRepository(FilyzerDbContext context) 
        {
            _context = context;
        }
        public async Task<User> GetByApiKeyAsync(string apiKey)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ApiKey == apiKey);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

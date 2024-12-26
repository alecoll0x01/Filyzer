using System.Data;
using Filyzer.Domain.Enums;

namespace Filyzer.Domain.Entities
{
    public class User
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; } = false;
        public UserRole Role { get; private set; }
        public string ApiKey { get; private set; }
        public int ApiCallsCount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { }
        public User(string email, UserRole role = UserRole.Basic)
        {
            Id = Guid.NewGuid();
            Email = email;
            Active = false;
            ApiKey = GetGenerateApiKey();
            Role = role;
            CreatedAt = DateTime.UtcNow;
            ApiCallsCount = 0;
        }

        public void UpdateRole(UserRole newRole)
        {
            Role = newRole;
        }

        private string GetGenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public void IncrementApiCalls()
        {
            ApiCallsCount++;
        }
    }
}
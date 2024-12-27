using Filyzer.Domain.Entities;
using Filyzer.Domain.Enums;

namespace Filyzer.Domain.Models.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string ApiKey { get; set; }
        public UserRole Role { get; set; }
        public int ApiCallsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DailyRequestLimit { get; set; }

        public static UserResponse FromUser(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                ApiKey = user.ApiKey,
                Role = user.Role,
                ApiCallsCount = user.ApiCallsCount,
                CreatedAt = user.CreatedAt,
                DailyRequestLimit = user.ApiCallsCount * 1000
            };
        }
    }

}

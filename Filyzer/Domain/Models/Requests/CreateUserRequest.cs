using Filyzer.Domain.Enums;

namespace Filyzer.Domain.Models.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.Basic;
        public int DailyRequestLimit { get; set; }
    }

}

namespace Filyzer.Domain.Models.Requests
{
    public class UpdateUserRequest
    {
        public string Email { get; set; }
        public int? DailyRequestLimit { get; set; }
    }

}

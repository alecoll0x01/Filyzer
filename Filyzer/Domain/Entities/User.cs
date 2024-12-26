namespace Filyzer.Domain.Entities
{
    public class User
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; } = false;
        public string ApiKey { get; private set; }
        public int ApiCallsCount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { }
        public User(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
            Active = false;
            ApiKey = GenerateApiKey();
            CreatedAt = DateTime.UtcNow;
            ApiCallsCount = 0;
        }

        private string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
        public void IncrementApiCalls()
        {
            ApiCallsCount++;
        }
    }
}
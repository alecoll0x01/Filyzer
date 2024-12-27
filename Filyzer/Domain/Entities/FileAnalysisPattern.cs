namespace Filyzer.Domain.Entities
{
    public class FileAnalysisPattern
    {
        public Guid Id { get; private set; }
        public string PatternName { get; private set; }
        public string PatternDescription { get; private set; }
        public string PatternSignature { get; private set; }
        public PatternType Type { get; private set; }
        public int RiskLevel { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public FileAnalysisPattern(string name, string description, string signature, PatternType type, int riskLevel)
        {
            Id = Guid.NewGuid();
            PatternName = name;
            PatternDescription = description;
            PatternSignature = signature;
            Type = type;
            RiskLevel = riskLevel;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }

    public enum PatternType
    {
        StringMatch,
        RegexPattern,
        ByteSequence,
        APICall
    }
}

namespace API.Models.UserImpression
{
    public class UserImpressionModel
    {
        public Guid UserId { get; init; }
        public string Impression { get; set; } = null!;
    }
}

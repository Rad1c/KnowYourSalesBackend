namespace API.Models.RegisterUser
{
    public record RegisterUserModel
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Sex { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string DateOfBirth { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string ConfirmPassword { get; init; } = null!;
    }
}

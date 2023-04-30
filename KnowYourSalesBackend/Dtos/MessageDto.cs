namespace API.Dtos
{
    public record MessageDto
    {
        public string Message { get; init; } = null!;

        public MessageDto(string message) => Message = message;
    }
}

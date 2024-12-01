namespace PasswordManager.Models
{
    public record PasswordToShowModel
    {
        public int Id { get; init; } = 0;
        public string Username { get; init; } = string.Empty;
        public char[] Password { get; init; } = [];
        public string Url { get; init; } = string.Empty;
        public string ExpirationDate { get; init; } = string.Empty;
        public string CategoryPath { get; init; } = string.Empty;
        public string Tags { get; init; } = string.Empty;
        public bool Favorite { get; init; } = false;
        public string Notes { get; init; } = string.Empty;
    }
}

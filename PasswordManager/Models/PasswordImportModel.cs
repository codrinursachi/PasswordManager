namespace PasswordManager.Models
{
    public record PasswordImportModel
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public DateTime? ExpirationDate { get; init; }
        public string CategoryPath { get; init; } = string.Empty;
        public string Tags { get; init; } = string.Empty;
        public bool Favorite { get; init; } = false;
        public string Notes { get; init; } = string.Empty;
    }
}

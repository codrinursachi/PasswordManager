namespace PasswordManager.Models
{
    public record PasswordModel
    {
        public int Id { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public char[] Password { get; set; } = [];
        public string Url { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public string? CategoryPath { get; set; } = string.Empty;
        public string? Tags { get; set; } = string.Empty;
        public bool Favorite { get; set; } = false;
        public string? Notes { get; set; } = string.Empty;
    }
}

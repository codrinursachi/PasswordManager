namespace PasswordManager.Models
{
    public record PasswordImportModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public string CategoryPath { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public bool Favorite { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
    }
}

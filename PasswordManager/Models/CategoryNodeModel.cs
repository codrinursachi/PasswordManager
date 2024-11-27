namespace PasswordManager.Models
{
    public record CategoryNodeModel
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public CategoryNodeModel Parent { get; set; }
        public List<CategoryNodeModel> Children { get; set; } = [];
    }
}

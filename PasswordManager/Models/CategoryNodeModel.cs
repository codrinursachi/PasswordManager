namespace PasswordManager.Models
{
    public record CategoryNodeModel
    {
        public string Name { get; init; }
        public int Level { get; init; }
        public CategoryNodeModel Parent { get; init; }
        public List<CategoryNodeModel> Children { get; init; } = [];
    }
}

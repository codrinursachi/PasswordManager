using PasswordManager.Models;

namespace PasswordManagerTests.Models
{
    public class CategoryNodeModelTests
    {
        [Fact]
        public void ShouldStoreCategoryNodeData()
        {
            CategoryNodeModel parent = new();
            CategoryNodeModel categoryNodeModel = new()
            {
                Parent = parent,
                Name = "Test"
            };
            CategoryNodeModel child1 = new();
            CategoryNodeModel child2 = new();
            categoryNodeModel.Children.AddRange([child1, child2]);
            Assert.Equal("Test", categoryNodeModel.Name);
            Assert.Equal(parent, categoryNodeModel.Parent);
            Assert.Equal(child1, categoryNodeModel.Children[0]);
            Assert.Equal(child2, categoryNodeModel.Children[1]);
        }
    }
}

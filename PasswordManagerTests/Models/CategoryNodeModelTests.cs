using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Models
{
    public class CategoryNodeModelTests
    {
        [Fact]
        public void ShouldStoreCategoryNodeData()
        {
            CategoryNodeModel categoryNodeModel = new();
            CategoryNodeModel parent = new();
            CategoryNodeModel child1 = new();
            CategoryNodeModel child2 = new();
            categoryNodeModel.Parent = parent;
            categoryNodeModel.Children.AddRange([child1, child2]);
            categoryNodeModel.Name = "Test";
            Assert.Equal("Test", categoryNodeModel.Name);
            Assert.Equal(parent, categoryNodeModel.Parent);
            Assert.Equal(child1, categoryNodeModel.Children[0]);
            Assert.Equal(child2, categoryNodeModel.Children[1]);
        }
    }
}

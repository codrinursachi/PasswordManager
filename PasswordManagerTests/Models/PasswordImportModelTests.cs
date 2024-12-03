using PasswordManager.Models;

namespace PasswordManagerTests.Models
{
    public class PasswordImportModelTests
    {
        [Fact]
        public void ShouldStorePasswordImportModelData()
        {
            PasswordImportModel password = new()
            {
                Username = "admin",
                Password = "admin",
                Url = "admin.com",
                ExpirationDate = null,
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            Assert.Equal("admin", password.Username);
            Assert.Equal("admin", password.Password);
            Assert.Equal("admin.com", password.Url);
            Assert.Equal(null, password.ExpirationDate);
            Assert.Equal("admin/admin", password.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", password.Tags);
            Assert.True(password.Favorite);
            Assert.Equal("admin notes", password.Notes);
        }
    }
}

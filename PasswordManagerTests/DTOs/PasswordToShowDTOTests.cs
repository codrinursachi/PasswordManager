using PasswordManager.DTO;

namespace PasswordManagerTests.DTOs
{
    public class PasswordToShowDTOTests
    {

        [Fact]
        public void ShouldStorePasswordDTOData()
        {
            PasswordToShowDTO password = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = "No expiration",
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            Assert.Equal(1, password.Id);
            Assert.Equal("admin", password.Username);
            Assert.Equal("admin".ToCharArray(), password.Password);
            Assert.Equal("admin.com", password.Url);
            Assert.Equal("No expiration", password.ExpirationDate);
            Assert.Equal("admin/admin", password.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", password.Tags);
            Assert.True(password.Favorite);
            Assert.Equal("admin notes", password.Notes);
        }
    }
}

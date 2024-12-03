using PasswordManager.Models;
using PasswordManager.Models.Extensions;

namespace PasswordManagerTests.Models
{
    public class PasswordModelExtensionsTests
    {
        [Fact]
        public void ShouldConvertToPasswordToShowModelWhenNoExpirationDate()
        {
            PasswordModel passwordModel = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = default,
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            PasswordToShowModel password = passwordModel.ToPasswordToShowModel();
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

        [Fact]
        public void ShouldConvertToPasswordToShowModelWhenPasswordExpires()
        {
            PasswordModel passwordModel = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = DateTime.Now + TimeSpan.FromDays(1),
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            PasswordToShowModel password = passwordModel.ToPasswordToShowModel();
            Assert.Equal(1, password.Id);
            Assert.Equal("admin", password.Username);
            Assert.Equal("admin".ToCharArray(), password.Password);
            Assert.Equal("admin.com", password.Url);
            Assert.Equal((DateTime.Now + TimeSpan.FromDays(1)).ToShortDateString(), password.ExpirationDate);
            Assert.Equal("admin/admin", password.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", password.Tags);
            Assert.True(password.Favorite);
            Assert.Equal("admin notes", password.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordToShowModelWhenPasswordExpired()
        {
            PasswordModel passwordModel = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = DateTime.Now - TimeSpan.FromDays(1),
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            PasswordToShowModel passwordDTO = passwordModel.ToPasswordToShowModel();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin".ToCharArray(), passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal("Expired", passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordDTO.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordDTO.Notes);
        }

        [Fact]
        public void ShouldConvertPasswordImportModelToPasswordModel()
        {
            PasswordImportModel PasswordImportDTO = new()
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
            PasswordModel passwordModel = PasswordImportDTO.ToPasswordModel();
            Assert.Equal("admin", passwordModel.Username);
            Assert.Equal("admin".ToCharArray(), passwordModel.Password);
            Assert.Equal("admin.com", passwordModel.Url);
            Assert.Equal(null, passwordModel.ExpirationDate);
            Assert.Equal("admin/admin", passwordModel.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordModel.Tags);
            Assert.True(passwordModel.Favorite);
            Assert.Equal("admin notes", passwordModel.Notes);
        }
    }
}
using PasswordManager.Models;
using PasswordManager.Models.Extensions;

namespace PasswordManagerTests.DTOs
{
    public class PasswordDTOExtensionsTests
    {
        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenNoExpirationDate()
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
            PasswordToShowModel passwordDTO = passwordModel.ToPasswordToShowModel();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin".ToCharArray(), passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal("No expiration", passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordDTO.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordDTO.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenPasswordExpires()
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
            PasswordToShowModel passwordDTO = passwordModel.ToPasswordToShowModel();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin".ToCharArray(), passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal((DateTime.Now + TimeSpan.FromDays(1)).ToShortDateString(), passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordDTO.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordDTO.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenPasswordExpired()
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
        public void ShouldConvertPasswordImportDTOToPasswordModel()
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
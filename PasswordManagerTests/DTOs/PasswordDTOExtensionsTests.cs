using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
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
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
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
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal("Expired", passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordDTO.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordDTO.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordModelWhenPasswordExpired()
        {
            PasswordToShowDTO passwordDTO = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = "Expired",
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            PasswordModel passwordModel = passwordDTO.ToPasswordModel();
            Assert.Equal(1, passwordModel.Id);
            Assert.Equal("admin", passwordModel.Username);
            Assert.Equal("admin", passwordModel.Password);
            Assert.Equal("admin.com", passwordModel.Url);
            Assert.Equal(default, passwordModel.ExpirationDate);
            Assert.Equal("admin/admin", passwordModel.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordModel.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordModel.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordModelWhenPasswordNotExpired()
        {
            PasswordToShowDTO passwordDTO = new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin".ToCharArray(),
                Url = "admin.com",
                ExpirationDate = (DateTime.Now+TimeSpan.FromDays(7)).ToShortDateString(),
                CategoryPath = "admin/admin",
                Tags = "#tag1 #tag2 #tag3",
                Favorite = true,
                Notes = "admin notes"
            };
            PasswordModel passwordModel = passwordDTO.ToPasswordModel();
            Assert.Equal(1, passwordModel.Id);
            Assert.Equal("admin", passwordModel.Username);
            Assert.Equal("admin", passwordModel.Password);
            Assert.Equal("admin.com", passwordModel.Url);
            Assert.Equal(DateTime.Now + TimeSpan.FromDays(7), passwordModel.ExpirationDate);
            Assert.Equal("admin/admin", passwordModel.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordModel.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordModel.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordModelWhenPasswordHasNoExpirationDate()
        {
            PasswordToShowDTO passwordDTO = new()
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
            PasswordModel passwordModel = passwordDTO.ToPasswordModel();
            Assert.Equal(1, passwordModel.Id);
            Assert.Equal("admin", passwordModel.Username);
            Assert.Equal("admin", passwordModel.Password);
            Assert.Equal("admin.com", passwordModel.Url);
            Assert.Equal(default, passwordModel.ExpirationDate);
            Assert.Equal("admin/admin", passwordModel.CategoryPath);
            Assert.Equal("#tag1 #tag2 #tag3", passwordModel.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordModel.Notes);
        }
    }
}
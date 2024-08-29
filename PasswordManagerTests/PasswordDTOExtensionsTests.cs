using PasswordManager.DTO;
using PasswordManager.DTO.Extensions;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests
{
    public class PasswordDTOExtensionsTests
    {
        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenNoExpirationDate()
        {
            PasswordModel passwordModel = new();
            passwordModel.Id = 1;
            passwordModel.Username = "admin";
            passwordModel.Password = "admin";
            passwordModel.Url = "admin.com";
            passwordModel.ExpirationDate = default;
            passwordModel.CategoryPath = "admin/admin";
            passwordModel.Tags = "tag1 tag2 tag3";
            passwordModel.Favorite = true;
            passwordModel.Notes = "admin notes";
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal("No expiration", passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("tag1 tag2 tag3", passwordDTO.Tags);
            Assert.True(passwordDTO.Favorite);
            Assert.Equal("admin notes", passwordDTO.Notes);
        }

        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenPasswordExpires()
        {
            PasswordModel passwordModel = new();
            passwordModel.Id = 1;
            passwordModel.Username = "admin";
            passwordModel.Password = "admin";
            passwordModel.Url = "admin.com";
            passwordModel.ExpirationDate = DateTime.Now + TimeSpan.FromDays(1);
            passwordModel.CategoryPath = "admin/admin";
            passwordModel.Tags = "tag1 tag2 tag3";
            passwordModel.Favorite = true;
            passwordModel.Notes = "admin notes";
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal((DateTime.Now + TimeSpan.FromDays(1)).ToShortDateString(), passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("tag1 tag2 tag3", passwordDTO.Tags);
        }

        [Fact]
        public void ShouldConvertToPasswordToShowDTOWhenPasswordExpired()
        {
            PasswordModel passwordModel = new();
            passwordModel.Id = 1;
            passwordModel.Username = "admin";
            passwordModel.Password = "admin";
            passwordModel.Url = "admin.com";
            passwordModel.ExpirationDate = DateTime.Now - TimeSpan.FromDays(1);
            passwordModel.CategoryPath = "admin/admin";
            passwordModel.Tags = "tag1 tag2 tag3";
            passwordModel.Favorite = true;
            passwordModel.Notes = "admin notes";
            PasswordToShowDTO passwordDTO = passwordModel.ToPasswordToShowDTO();
            Assert.Equal(1, passwordDTO.Id);
            Assert.Equal("admin", passwordDTO.Username);
            Assert.Equal("admin", passwordDTO.Password);
            Assert.Equal("admin.com", passwordDTO.Url);
            Assert.Equal("expired", passwordDTO.ExpirationDate);
            Assert.Equal("admin/admin", passwordDTO.CategoryPath);
            Assert.Equal("tag1 tag2 tag3", passwordDTO.Tags);
        }
    }
}
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Models
{
    public class PasswordModelTests
    {
        [Fact]
        public void ShouldStorePasswordData()
        {
            PasswordModel password = new();
            password.Id = 1;
            password.Username = "admin";
            password.Password = "admin".ToCharArray();
            password.Url = "admin.com";
            password.ExpirationDate = default;
            password.CategoryPath = "admin/admin";
            password.Tags = "tag1 tag2 tag3";
            password.Favorite = true;
            password.Notes = "admin notes";
            Assert.Equal(1, password.Id);
            Assert.Equal("admin", password.Username);
            Assert.Equal("admin".ToCharArray(), password.Password);
            Assert.Equal("admin.com", password.Url);
            Assert.Equal(default, password.ExpirationDate);
            Assert.Equal("admin/admin", password.CategoryPath);
            Assert.Equal("tag1 tag2 tag3", password.Tags);
            Assert.True(password.Favorite);
            Assert.Equal("admin notes", password.Notes);
        }
    }
}

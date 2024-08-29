using PasswordManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests
{
    public class PasswordToShowDTOTests
    {

        [Fact]
        public void ShouldStorePasswordDTOData()
        {
            PasswordToShowDTO password = new();
            password.Id = 1;
            password.Username = "admin";
            password.Password = "admin";
            password.Url = "admin.com";
            password.ExpirationDate = "No expiration";
            password.CategoryPath = "admin/admin";
            password.Tags = "tag1 tag2 tag3";
            password.Favorite = true;
            password.Notes = "admin notes";
            Assert.Equal(1, password.Id);
            Assert.Equal("admin", password.Username);
            Assert.Equal("admin", password.Password);
            Assert.Equal("admin.com", password.Url);
            Assert.Equal("No expiration", password.ExpirationDate);
            Assert.Equal("admin/admin", password.CategoryPath);
            Assert.Equal("tag1 tag2 tag3", password.Tags);
            Assert.True(password.Favorite);
            Assert.Equal("admin notes", password.Notes);
        }
    }
}

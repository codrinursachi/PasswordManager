using PasswordManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.DTOs
{
    public class PasswordImportDTOTests
    {
        [Fact]
        public void ShouldStorePasswordImportDTOData()
        {
            PasswordImportDTO password = new()
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

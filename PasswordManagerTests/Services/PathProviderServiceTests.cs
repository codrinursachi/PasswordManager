using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Services
{
    public class PathProviderServiceTests
    {
        [Fact]
        public void ShouldReturnCorrectPath()
        {
            PathProviderService pathProviderService = new();
            Assert.Equal(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager"),pathProviderService.ProgramPath);
        }
    }
}

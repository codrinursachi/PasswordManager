using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerTests.Services
{
    public class ModalDialogResultProviderServiceTests
    {
        [Fact]
        public void ShouldStoreResult()
        {
            ModalDialogResultProviderService modalDialogResultProviderService = new()
            {
                Result = true
            };

            Assert.True(modalDialogResultProviderService.Result);
        }
    }
}

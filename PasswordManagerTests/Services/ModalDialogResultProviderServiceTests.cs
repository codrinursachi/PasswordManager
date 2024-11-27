using PasswordManager.Services;

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

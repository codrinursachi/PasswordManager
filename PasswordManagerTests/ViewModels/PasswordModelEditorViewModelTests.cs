namespace PasswordManagerTests.ViewModels
{
    public class PasswordModelEditorViewModelTests
    {
        [Fact]
        public void ShouldStorePassword()
        {
            //string file = Path.Combine("Temporary", Path.GetRandomFileName());
            //DatabaseInfoProviderService databaseInfoProviderService = new()
            //{
            //    CurrentDatabase = file,
            //    DBPass = ProtectedData.Protect(Encoding.UTF8.GetBytes("admin"), null, DataProtectionScope.CurrentUser)
            //};
            //DatabaseStorageService databaseStorageService = new("Temporary");
            //PasswordRepository passwordRepository = new(databaseInfoProviderService);
            //ModalDialogProviderService modalDialogProviderService = new();
            //PasswordModelEditorViewModel passwordCreationViewModel = new(databaseInfoProviderService,databaseStorageService,modalDialogProviderService)
            //{
            //    PasswordAsCharArray = "admin".ToCharArray(),
            //    Database = file,
            //    Username = "admin",
            //    Url = "admin.com",
            //};
            //passwordCreationViewModel.AddPassword(null);
            //PasswordRepository passwordRepository = new(file, dBPass);
            //Assert.Single(passwordRepository.GetAllPasswords());
            //File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager", "Databases", file + ".json"));
        }
    }
}

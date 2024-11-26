using PasswordManager.Models;

namespace PasswordManager.DTO.Extensions
{
    public static class PasswordDTOExtensions
    {
        public static PasswordToShowDTO ToPasswordToShowDTO(this PasswordModel passwordModel)
        {
            return new PasswordToShowDTO
            {
                Id = passwordModel.Id,
                Favorite = passwordModel.Favorite,
                Url = passwordModel.Url,
                Username = passwordModel.Username,
                Password = passwordModel.Password,
                ExpirationDate = passwordModel.ExpirationDate == default ? "No expiration" : passwordModel.ExpirationDate < DateTime.Now ? "Expired" : passwordModel.ExpirationDate?.ToShortDateString(),
                CategoryPath = passwordModel.CategoryPath,
                Tags = passwordModel.Tags,
                Notes = passwordModel.Notes
            };
        }

        public static PasswordModel ToPasswordModel(this PasswordImportDTO passwordToImport)
        {
            return new PasswordModel
            {
                Favorite = passwordToImport.Favorite,
                Url = passwordToImport.Url,
                Username = passwordToImport.Username,
                Password = passwordToImport.Password.ToCharArray(),
                ExpirationDate = passwordToImport.ExpirationDate,
                CategoryPath = passwordToImport.CategoryPath,
                Tags = passwordToImport.Tags,
                Notes = passwordToImport.Notes
            };
        }
    }
}

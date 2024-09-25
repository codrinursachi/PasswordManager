using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ExpirationDate = passwordModel.ExpirationDate == default ? "No expiration" : passwordModel.ExpirationDate < DateTime.Now ? "Expired" : passwordModel.ExpirationDate.ToShortDateString(),
                CategoryPath = passwordModel.CategoryPath,
                Tags = passwordModel.Tags,
                Notes = passwordModel.Notes
            };
        }

        public static PasswordModel ToPasswordModel(this PasswordToShowDTO passwordToShowDTO)
        {
            return new PasswordModel
            {
                Id = passwordToShowDTO.Id,
                Favorite = passwordToShowDTO.Favorite,
                Url = passwordToShowDTO.Url,
                Username = passwordToShowDTO.Username,
                Password = passwordToShowDTO.Password,
                ExpirationDate = passwordToShowDTO.ExpirationDate == "No expiration" ? default : DateTime.Parse(passwordToShowDTO.ExpirationDate),
                CategoryPath = passwordToShowDTO.CategoryPath,
                Tags = passwordToShowDTO.Tags,
                Notes = passwordToShowDTO.Notes
            };
        }
    }
}

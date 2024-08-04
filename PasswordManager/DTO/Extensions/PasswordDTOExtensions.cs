using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DTO.Extensions
{
    internal static class PasswordDTOExtensions
    {
        public static PasswordToShowDTO ToPasswordToShow(this PasswordModel passwordModel)
        {
            return new PasswordToShowDTO
            {
                Favorite = passwordModel.Favorite,
                Url = passwordModel.Url,
                Username = passwordModel.Username,
                Password = passwordModel.Password,
                ExpirationDate = passwordModel.ExpirationDate == default ? "No expiration" : passwordModel.ExpirationDate < DateTime.Now ? "expired" : passwordModel.ExpirationDate.ToShortDateString(),
                CategoryPath = passwordModel.CategoryPath,
                Tags = passwordModel.Tags,
                Notes = passwordModel.Notes
            };
        }
    }
}

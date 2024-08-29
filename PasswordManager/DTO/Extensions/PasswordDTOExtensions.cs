﻿using PasswordManager.Models;
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
                ExpirationDate = passwordModel.ExpirationDate == default ? "No expiration" : passwordModel.ExpirationDate < DateTime.Now ? "expired" : passwordModel.ExpirationDate.ToShortDateString(),
                CategoryPath = passwordModel.CategoryPath,
                Tags = passwordModel.Tags,
                Notes = passwordModel.Notes
            };
        }
    }
}

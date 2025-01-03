﻿using CommunityToolkit.Mvvm.Messaging;
using Moq;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Models.Extensions;
using PasswordManager.ViewModels;

namespace PasswordManagerTests.ViewModels
{
    public class CategoryViewModelTests
    {
        [Fact]
        public void ShouldStorePasswords()
        {
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", CategoryPath = "admin\\1" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", CategoryPath = "admin\\2" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", CategoryPath = "admin\\3" };
            var passwordListMessenger = new Mock<IMessenger>();
            var passwordManagementService = new Mock<IPasswordManagementService>();
            passwordManagementService.Setup(m => m.GetAllPasswords())
                                     .Returns(new List<PasswordToShowModel>([password.ToPasswordToShowModel(), password2.ToPasswordToShowModel(), password3.ToPasswordToShowModel()]));

            CategoryViewModel categoryViewModel = new(passwordManagementService.Object, passwordListMessenger.Object);
            categoryViewModel.Refresh();
            Assert.Equal(3, categoryViewModel.Passwords.Count);
        }

        [Fact]
        public void ShouldFilterPasswords()
        {
            PasswordModel password = new() { Username = "admin", Password = "admin".ToCharArray(), Url = "admin.com", CategoryPath = "admin\\1" };
            PasswordModel password2 = new() { Username = "admin2", Password = "admin2".ToCharArray(), Url = "admin2.com", CategoryPath = "admin\\2" };
            PasswordModel password3 = new() { Username = "admin3", Password = "admin3".ToCharArray(), Url = "admin3.com", CategoryPath = "admin\\3" };
            var passwordListMessenger = new Mock<IMessenger>();
            var passwordManagementService = new Mock<IPasswordManagementService>();
            passwordManagementService.Setup(m => m.GetAllPasswords())
                                     .Returns(new List<PasswordToShowModel>([password.ToPasswordToShowModel(), password2.ToPasswordToShowModel(), password3.ToPasswordToShowModel()]));

            CategoryViewModel categoryViewModel = new(passwordManagementService.Object, passwordListMessenger.Object);
            var root = new CategoryNodeModel { Name = "Categories" };
            var parts = "admin\\2".Split('\\');
            var current = root;
            foreach (var part in parts)
            {
                var child = current.Children.FirstOrDefault(p => p.Name == part);
                if (child == null)
                {
                    child = new CategoryNodeModel { Name = part, Parent = current };
                    current.Children.Add(child);
                }

                current = child;
            }

            categoryViewModel.Filter = current;
            Assert.Single(categoryViewModel.Passwords);
        }
    }
}

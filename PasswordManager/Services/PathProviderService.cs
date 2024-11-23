using PasswordManager.Interfaces;
using System.IO;

namespace PasswordManager.Services
{
    public class PathProviderService : IPathProviderService
    {
        public string ProgramPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PasswordManager");
    }
}

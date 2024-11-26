using PasswordManager.Interfaces;
using System.IO;

namespace PasswordManager.Services
{
    class ProgramFoldersCreatorService : IProgramFoldersCreatorService
    {
        IPathProviderService pathProviderService;
        public ProgramFoldersCreatorService(IPathProviderService pathProviderService)
        {
            this.pathProviderService = pathProviderService;
        }
        public void CreateFoldersIfNecessary()
        {
            List<string> folderNames = ["", "Databases", "Backups"];
            foreach (string folderName in folderNames)
            {
                var folderPath = Path.Combine(pathProviderService.ProgramPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
        }
    }
}

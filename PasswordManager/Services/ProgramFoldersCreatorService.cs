using PasswordManager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    class ProgramFoldersCreatorService : IProgramFoldersCreatorService
    {
        IPathProviderService pathProviderService;
        public ProgramFoldersCreatorService(IPathProviderService pathProviderService) { 
            this.pathProviderService = pathProviderService;
        }
        public void CreateFoldersIfNecessary()
        {
            List<string> folderNames = ["","Databases","Backups"];
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

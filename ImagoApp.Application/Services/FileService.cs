using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImagoApp.Application.Services
{
    public interface IFileService
    {
        string GetApplicationFolder();
        string GetCharacterDatabaseFolder();
    }

    public class FileService : IFileService
    {
        private const string CharacterDatabaseFolderName = "Characters";

        public string GetApplicationFolder()
        {
          return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public string GetCharacterDatabaseFolder()
        {
            var databaseFolder = Path.Combine(GetApplicationFolder(), CharacterDatabaseFolderName);
            if (!Directory.Exists(databaseFolder))
                Directory.CreateDirectory(databaseFolder);

            return databaseFolder;
        }
    }
}

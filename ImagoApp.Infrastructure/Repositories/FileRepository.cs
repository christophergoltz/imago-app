using System;
using System.IO;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IFileRepository
    {
        string GetApplicationFolder();
        string GetCharacterDatabaseFolder();
    }

    public class FileRepository : IFileRepository
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

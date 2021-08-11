using System;
using System.Diagnostics;
using System.IO;
using ImagoApp.Infrastructure.Repositories;

namespace ImagoApp.Infrastructure.Database
{
    public interface ICharacterDatabaseConnection
    {
        string GetCharacterDatabaseFile(Guid guid);
        string GetDatabaseConnectionString(Guid guid);
    }

    public class CharacterDatabaseConnection : ICharacterDatabaseConnection
    {
        private readonly string _databaseFolder;

        public CharacterDatabaseConnection(IFileRepository fileRepository)
        {
            _databaseFolder = fileRepository.GetCharacterDatabaseFolder();
            Debug.WriteLine($"DatabaseFolder: {_databaseFolder}");
        }
        
        public string GetCharacterDatabaseFile(Guid guid)
        {
            var fileName = $"{guid}.imagodb";
            var databaseFile = Path.Combine(_databaseFolder, fileName);
            return databaseFile;
        }

        public string GetDatabaseConnectionString(Guid guid)
        {
            var databaseFile = GetCharacterDatabaseFile(guid);
            return $"filename={databaseFile}";
        }
    }
}
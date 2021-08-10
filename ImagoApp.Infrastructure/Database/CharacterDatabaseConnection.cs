using System;
using System.IO;

namespace ImagoApp.Infrastructure.Database
{
    public interface ICharacterDatabaseConnection
    {
        string GetCharacterDatabaseFile(Guid guid);
    }

    public class CharacterDatabaseConnection : ICharacterDatabaseConnection
    {
        private readonly string _databaseFolder;

        public CharacterDatabaseConnection(string databaseFolder)
        {
            _databaseFolder = databaseFolder;
        }
        
        public string GetCharacterDatabaseFile(Guid guid)
        {
            var fileName = $"{guid}.imagodb";
            var databaseFile = Path.Combine(_databaseFolder, fileName);
            return databaseFile;
        }
    }
}
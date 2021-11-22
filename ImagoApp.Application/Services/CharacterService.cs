using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Database;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared;

namespace ImagoApp.Application.Services
{
    public interface ICharacterService
    {
        bool SaveCharacter(CharacterModel characterModel);
        bool AddCharacter(CharacterModel characterModel);
        IEnumerable<CharacterPreview> GetAllQuick();
        CharacterModel GetItem(Guid guid);
        bool Delete(Guid guid);
        string GetCharacterJson(Guid guid);
        bool ImportCharacter(CharacterEntity characterEntity);
        void UpdateLastBackup(Guid guid);
        CharacterPreview GetCharacterPreview(string dbFile);
        CharacterPreview GetCharacterPreview(Guid guid);
    }

    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;

        public CharacterService(ICharacterRepository characterRepository,
            IMapper mapper,
            IFileRepository fileRepository,
            ICharacterDatabaseConnection characterDatabaseConnection)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
            _fileRepository = fileRepository;
            _characterDatabaseConnection = characterDatabaseConnection;
        }

        public bool Delete(Guid guid)
        {
            var characterDatabaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(guid);

            try
            {
                if (File.Exists(characterDatabaseFile))
                {
                    File.Delete(characterDatabaseFile);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public CharacterModel GetItem(Guid guid)
        {
            var characterEntity = _characterRepository.GetItem(guid);
            return _mapper.Map<CharacterModel>(characterEntity);
        }

        public string GetCharacterJson(Guid guid)
        {
            return _characterRepository.GetCharacterJson(guid);
        }
        
        public CharacterPreview GetCharacterPreview(string dbFile)
        {
            return _characterRepository.GetItemAsPreview(dbFile);
        }

        public CharacterPreview GetCharacterPreview(Guid guid)
        {
            return _characterRepository.GetItemAsPreview(guid);
        }

        public IEnumerable<CharacterPreview> GetAllQuick()
        {
            var characterDatabaseFolder = _fileRepository.GetCharacterDatabaseFolder();
            foreach (var characterDatabaseFile in Directory.GetFiles(characterDatabaseFolder))
            {
                yield return _characterRepository.GetItemAsPreview(characterDatabaseFile);
            }
        }

        public bool SaveCharacter(CharacterModel characterModel)
        {
            Debug.WriteLine("Start saving..");
            var entity = _mapper.Map<CharacterEntity>(characterModel);
            var result = _characterRepository.UpdateItem(entity);
            Debug.WriteLine("Done saving: " + result);
            return result;
        }

        public bool ImportCharacter(CharacterEntity characterEntity)
        {
            return _characterRepository.InsertItem(characterEntity);
        }

        public void UpdateLastBackup(Guid guid)
        {
            _characterRepository.UpdateLastBackup(guid);
        }

        public bool AddCharacter(CharacterModel characterModel)
        {
            var entity = _mapper.Map<CharacterEntity>(characterModel);
            var result = _characterRepository.InsertItem(entity);
            return result;
        }
    }
}
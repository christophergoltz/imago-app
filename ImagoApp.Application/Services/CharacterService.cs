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
        CharacterModel GetItem(Guid id);
        bool Delete(Guid guid);
        string GetCharacterJson(Guid id);
        bool ImportCharacter(CharacterEntity characterEntity);
        void UpdateLastBackup(Guid guid);
    }

    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;

        public CharacterService(ICharacterRepository characterRepository, IMapper mapper, IFileService fileService, ICharacterDatabaseConnection characterDatabaseConnection)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
            _fileService = fileService;
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
        
        public CharacterModel GetItem(Guid id)
        {
            var characterDatabaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(id);
            var characterEntity = _characterRepository.GetItem(id,characterDatabaseFile);
            return _mapper.Map<CharacterModel>(characterEntity);
        }

        public string GetCharacterJson(Guid id)
        {
            return _characterRepository.GetCharacterJson(id);
        }

        public IEnumerable<CharacterPreview> GetAllQuick()
        {
            var characterDatabaseFolder = _fileService.GetCharacterDatabaseFolder();
            foreach (var characterDatabaseFile in Directory.GetFiles(characterDatabaseFolder))
            {
                var characterRepository = new CharacterRepository(() => characterDatabaseFile, _characterDatabaseConnection);
                yield return characterRepository.GetItemAsPreview();
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
            var characterDatabaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(characterEntity.Guid);
            return _characterRepository.InsertItem(characterEntity, characterDatabaseFile);
        }

        public void UpdateLastBackup(Guid guid)
        {

            var characterDatabaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(guid);
            _characterRepository.UpdateLastBackup(characterDatabaseFile);
        }

        public bool AddCharacter(CharacterModel characterModel)
        {
            var entity = _mapper.Map<CharacterEntity>(characterModel);
            var characterDatabaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(entity.Guid);
            var result = _characterRepository.InsertItem(entity, characterDatabaseFile);
            return result;
        }
    }
}
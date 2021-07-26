using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared;

namespace ImagoApp.Application.Services
{
    public interface ICharacterService
    {
        List<CharacterModel> GetAll();
        bool SaveCharacter(CharacterModel characterModel);
        bool AddCharacter(CharacterModel characterModel);
        FileInfo GetDatabaseInfo();
        List<CharacterItem> GetAllQuick();
        CharacterModel GetItem(Guid id);
        bool Delete(Guid guid);
        string GetCharacterJson(Guid id);
        bool ImportCharacter(CharacterEntity characterEntity);
    }

    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public CharacterService(ICharacterRepository characterRepository, IMapper mapper)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public bool Delete(Guid guid)
        {
            return _characterRepository.DeleteItem(guid);
        }

        public List<CharacterModel> GetAll()
        {
            var entities = _characterRepository.GetAllItems();
            return _mapper.Map<List<CharacterModel>>(entities);
        }

        public CharacterModel GetItem(Guid id)
        {
            var characterEntity = _characterRepository.GetItem(id);
            return _mapper.Map<CharacterModel>(characterEntity);
        }

        public string GetCharacterJson(Guid id)
        {
            return _characterRepository.GetCharacterJson(id);
        }

        public List<CharacterItem> GetAllQuick()
        {
            return _characterRepository.GetAllItemsQuick();
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

        public bool AddCharacter(CharacterModel characterModel)
        {
            var entity = _mapper.Map<CharacterEntity>(characterModel);
            var result = _characterRepository.InsertItem(entity);
            return result;
        }
        public FileInfo GetDatabaseInfo()
        {
            return _characterRepository.GetDatabaseInfo();
        }
    }
}
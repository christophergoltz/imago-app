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

        public bool AddCharacter(CharacterModel characterModel)
        {
            try
            {
                var entity = _mapper.Map<CharacterEntity>(characterModel);
                var result = _characterRepository.InsertItem(entity);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public FileInfo GetDatabaseInfo()
        {
            return _characterRepository.GetDatabaseInfo();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;

namespace ImagoApp.Application.Services
{
    public interface ICharacterService
    {
        List<Character> GetAll();
        bool SaveCharacter(Character character);
        bool AddCharacter(Character character);
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

        public List<Character> GetAll()
        {
            var entities = _characterRepository.GetAllItems();
            return _mapper.Map<List<Character>>(entities);
        }

        public bool SaveCharacter(Character character)
        {
            Debug.WriteLine("Start saving..");
            var entity = _mapper.Map<CharacterEntity>(character);
            var result = _characterRepository.UpdateItem(entity);
            Debug.WriteLine("Done saving..");
            return result;
        }

        public bool AddCharacter(Character character)
        {
            try
            {
                var entity = _mapper.Map<CharacterEntity>(character);
                var result = _characterRepository.InsertItem(entity);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
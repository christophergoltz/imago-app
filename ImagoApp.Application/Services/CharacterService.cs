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
        Task<List<Character>> GetAll();
        Task<bool> SaveCharacter(Character character);
        Task<bool> AddCharacter(Character character);
        Task Initialize();
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

        public async Task<List<Character>> GetAll()
        {
            var entities = await _characterRepository.GetAllItems();
            return _mapper.Map<List<Character>>(entities);
        }

        public async Task<bool> SaveCharacter(Character character)
        {
            Debug.WriteLine("Start saving..");
            var entity = _mapper.Map<CharacterEntity>(character);
            var result = await _characterRepository.UpdateItem(entity) == 1;
            Debug.WriteLine("Done saving..");

            return result;
        }

        public async Task<bool> AddCharacter(Character character)
        {
            var entity = _mapper.Map<CharacterEntity>(character);
            var result = await _characterRepository.AddItem(entity) == 1;
            return result;
        }

        public async Task Initialize()
        {
            await _characterRepository.EnsureTables();
        }
    }
}
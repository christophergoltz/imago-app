using System.Diagnostics;
using System.Threading.Tasks;

namespace ImagoApp.Services
{
    public interface ICharacterService
    {
        void SetCurrentCharacter(ViewModels.CharacterViewModel character);
        ViewModels.CharacterViewModel GetCurrentCharacter();
        Task<bool> SaveCurrentCharacter();
    }

    public class CharacterService : ICharacterService
    {
        private readonly Repository.WrappingDatabase.ICharacterRepository _characterRepository;

        public CharacterService(Repository.WrappingDatabase.ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        private ViewModels.CharacterViewModel _currentCharacter;

        public void SetCurrentCharacter(ViewModels.CharacterViewModel character)
        {
            _currentCharacter = character;
        }

        public async Task<bool> SaveCurrentCharacter()
        {
            Debug.WriteLine("Start saving..");
            var result = await _characterRepository.Update(_currentCharacter.Character);
            Debug.WriteLine("Done saving..");

            return result;
        }
        
        public ViewModels.CharacterViewModel GetCurrentCharacter()
        {
            return _currentCharacter;
        }
    }
}
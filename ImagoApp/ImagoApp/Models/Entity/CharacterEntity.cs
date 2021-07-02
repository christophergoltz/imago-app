using System;
using Newtonsoft.Json;
using SQLite;

namespace ImagoApp.Models.Entity
{
    public class CharacterEntity : IJsonValueWrapper<Character>
    {
        public CharacterEntity()
        {

        }

        [PrimaryKey]
        public Guid Id { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }

        public string ValueAsJson { get; set; }

        [Ignore]
        public Character Value
        {
            get => JsonConvert.DeserializeObject<Character>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}

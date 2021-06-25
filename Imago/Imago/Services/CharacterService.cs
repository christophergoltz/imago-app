using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Imago.Models;
using Imago.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Imago.Services
{
    public interface ICharacterService
    {
        void SetCurrentCharacter(CharacterViewModel character);
        CharacterViewModel GetCurrentCharacter();
    }

    public class CharacterService : ICharacterService
    {
        public CharacterService()
        {

        }

        private CharacterViewModel _currentCharacter;

        private Timer _timer;

        public void SetCurrentCharacter(CharacterViewModel character)
        {
            _currentCharacter = character;
            Register(_currentCharacter);

            _timer = new Timer(5000);
            _timer.Elapsed += SaveTimerOnElapsed;
            _timer.AutoReset = false;
        }

        private void SaveTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("Saving..");
        }
        

        public CharacterViewModel GetCurrentCharacter()
        {
            return _currentCharacter;
        }


        private void Register(object obj)
        {
            if (obj is INotifyPropertyChanged inpcObj)
            {
                inpcObj.PropertyChanged += (sender, args) => { CharacterPropertyUpdated(args.PropertyName); };
            }

            var t = obj.GetType().GetProperties();
            foreach (var propertyInfo in t)
            {
                if (System.Attribute.IsDefined(propertyInfo, typeof(JsonIgnoreAttribute)))
                {
                    continue;
                }

                if (propertyInfo.PropertyType == typeof(Guid))
                    continue;
                if (propertyInfo.PropertyType == typeof(string))
                    continue;
                if (propertyInfo.PropertyType == typeof(int))
                    continue;
                if (propertyInfo.PropertyType == typeof(Color))
                    continue;
                if (propertyInfo.PropertyType == typeof(System.Drawing.Color))
                    continue;

                if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var tt = propertyInfo.GetValue(obj);
                    if (tt is IEnumerable e)
                    {
                        foreach (var item in e)
                        {
                            Register(item);
                        }
                    }

                    continue;
                }

                var child = propertyInfo.GetValue(obj);
                Register(child);
            }
        }

        private void CharacterPropertyUpdated(string propertyName)
        {
            Debug.WriteLine("Char changed: " + propertyName);

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Start();
            }
        }
    }
}
using System;

namespace STGames
{
    public interface IStorageKey
    {
        public object DefaultValueAsObject { get; }
        string Key { get; }
        Type Type { get; }
        bool IsLanguageSensitive { get; }
        string KeyByLanguage(Language language);
    }


    public class StorageKey<T> : IStorageKey 
    {
        public event Action<T> ValueListeners;
        public string Name { get; set; }
        public bool IsLanguageSensitive { get; set; }
        public bool IsLevelSensitive { get; set; }
        public virtual T DefaultValue { get; set; }
        public object DefaultValueAsObject
        {
            get
            {
                return DefaultValue;
            }
        }
        public string Key
        {
            get
            {
                string result = Name;
                if (IsLanguageSensitive) result += $"_{AS.Language}";
                if (IsLevelSensitive) result += $"_{AS.Level}";
                return result;
            }
        }

        public string KeyByLanguage(Language language)
        {
            string result = Name;
            if (IsLanguageSensitive) result += $"_{language}";
            if (IsLevelSensitive) result += $"_{AS.Level}";
            return result;
        }

        public string KeyByLanguageLevel(Language language, int level)
        {
            string result = Name;
            if (IsLanguageSensitive) result += $"_{language}";
            if (IsLevelSensitive) result += $"_{level}";
            return result;
        }

        public Type Type
        {
            get
            {
                return typeof(T);
            }
        }


        public void InvokeListeners(T newValue)
        {
            ValueListeners?.Invoke(newValue);
        }
    }


    public static class SKUtil
    {
        public static StorageKey<T> CreateKey<T>(string name, T defaultValue, bool isLanguageSensitive = false, bool isLevelSensitive = false)
        {
            if (isLevelSensitive) isLanguageSensitive = true;
            return new StorageKey<T>()
            {
                IsLanguageSensitive = isLanguageSensitive,
                IsLevelSensitive = isLevelSensitive,
                Name = name,
                DefaultValue = defaultValue,
            };
        }
    }
}

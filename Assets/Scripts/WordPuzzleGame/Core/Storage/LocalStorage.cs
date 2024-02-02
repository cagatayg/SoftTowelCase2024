using System;

namespace STGames
{
    public static class LS
    {
        public static T Get<T>(string key, T defaultValue = default)
        {
            return LocalStorageManager.Get<T>(key, defaultValue);
        }

        public static void Set<T>(string key, T value)
        {
            LocalStorageManager.Set(key, value);
        }

        public static T GetByLanguage<T>(StorageKey<T> storageKey, Language language)
        {
            return LocalStorageManager.Get<T>(storageKey.KeyByLanguage(language), storageKey.DefaultValue);
        }

        public static T Get<T>(StorageKey<T> storageKey)
        {
            return LocalStorageManager.Get<T>(storageKey.Key, storageKey.DefaultValue);
        }

        public static void Set<T>(StorageKey<T> storageKey, T value)
        {
            LocalStorageManager.Set<T>(storageKey.Key, value);
            storageKey.InvokeListeners(value);
        }

        public static void Milestone(Milestone milestone)
        {
            var key = $"GameMilestone__{milestone}";
            if (!LocalStorageManager.Get(key, false))
            {
                LocalStorageManager.Set(key, true);
            }
        }

        public static void RemoveMilestone(Milestone milestone)
        {
            var key = $"GameMilestone__{milestone}";
            LocalStorageManager.Set(key, false);
        }

        public static bool HasMilestoneReached(Milestone milestone)
        {
            var key = $"GameMilestone__{milestone}";
            return LocalStorageManager.Get(key, false);
        }


        public static void RegisterListenerOn<T>(StorageKey<T> storageKey, Action<T> callback)
        {
            storageKey.ValueListeners += callback;
        }

        public static void RemoveListenerOn<T>(StorageKey<T> storageKey, Action<T> callback)
        {
            storageKey.ValueListeners -= callback;
        }
    }
}

using System;

namespace STGames
{
    public static class SK
    {
        public static readonly StorageKey<int> InstallationVersion = SKUtil.CreateKey("InstallationVersion", defaultValue: 0, isLanguageSensitive: false, isLevelSensitive: false);
        public static readonly StorageKey<DateTime> InstallationDate = SKUtil.CreateKey("InstallationDate", defaultValue: DateTime.MinValue, isLanguageSensitive: false, isLevelSensitive: false);
        public static readonly StorageKey<int> GoldAmount = SKUtil.CreateKey("GoldAmount", defaultValue: 0, isLanguageSensitive: false, isLevelSensitive: false);
        public static readonly StorageKey<int> Level = SKUtil.CreateKey("Level", defaultValue: 1, isLanguageSensitive: true, isLevelSensitive: false);
        public static readonly StorageKey<Language> Language = SKUtil.CreateKey("Language", defaultValue: STGames.Language.EN, isLanguageSensitive: false, isLevelSensitive: false);
    }
}
using System;

namespace STGames
{
    public static class LanguageUtil
    {
        public static string GetNativeLanguageName(Language language)
        {
            switch (language)
            {
                case Language.DE:
                    return "Deutsch";

                case Language.EN:
                    return "English";

                case Language.TR:
                    return "Türkçe";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }

    public enum Language : byte
    {
        DE,
        EN,
        TR
    }
}

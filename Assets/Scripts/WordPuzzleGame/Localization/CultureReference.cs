using System.Globalization;

namespace STGames
{
    public static class CultureReference
    {
        public static TextInfo GetTextInfo()
        {
            return GetCultureInfo(LSh.Language).TextInfo;
        }
        
        public static TextInfo GetTextInfo(Language language)
        {
            return GetCultureInfo(language).TextInfo;
        }
        public static CultureInfo GetCultureInfo(Language language)
        {
            switch (language)
            {
                case Language.TR:
                    return new CultureInfo("tr", false);
                case Language.DE:
                    return new CultureInfo("de", false);
                case Language.EN:
                    return new CultureInfo("en-US", false);
                default:
                    return new CultureInfo("en-US", false);
            }
        }
    }
}

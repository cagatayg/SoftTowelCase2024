using System;

namespace STGames
{
    public static class LSh
    {

        public static Language Language
        {
            get { return LS.Get(SK.Language); }
        }

        public static int CurrentLevel
        {
            get
            {
                AS.Level = LS.Get(SK.Level);
                return AS.Level;
            }
        }

        public static int CurrentGoldAmount
        {
            get { return LS.Get(SK.GoldAmount); }
        }
        

        public static int InstallationVersion
        {
            get { return LS.Get(SK.InstallationVersion); }
        }

        public static DateTime InstallationDate
        {
            get { return LS.Get(SK.InstallationDate); }
        }

        public static double HoursAfterInstall
        {
            get { return (DateTime.Now - LS.Get(SK.InstallationDate)).TotalHours; }
        }

   
        public static void SetLanguage(Language language)
        {
            LS.Milestone(Milestone.FirstLanguageSelected);
            LS.Set(SK.Language, language);
            AS.Language = language;
        }
    }
}
using System.Collections.Generic;

namespace STGames
{
    public static class GameBootstrap
    {
        public static CrosswordPuzzle CurrentPuzzle;
        public static List<BaseLetterBox> LetterBoxes;
        public static List<WheelLetterView> WheelLetters;
        public static bool IsLevelFinished;
    }
}
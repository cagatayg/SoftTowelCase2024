using System.Collections.Generic;
using Newtonsoft.Json;

namespace STGames
{
    public class Level
    {
        [JsonProperty("w")] public List<BoardWord> BoardWords;
        [JsonProperty("b")] public List<string> BonusWords;
        [JsonProperty("l")] public string WheelLetters;
        [JsonProperty("wi")] public int Width;
        [JsonProperty("h")] public int Height;
    }


    public class BoardWord
    {
        [JsonProperty("w")] public string Word;
        [JsonProperty("v")] public bool IsVertical;
        [JsonProperty("x")] public int X;
        [JsonProperty("y")] public int Y;
    }
}
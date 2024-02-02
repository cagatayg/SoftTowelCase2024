using Unity.Mathematics;
using UnityEngine;


using TMPro;

namespace STGames
{
    public abstract class BaseLetterBox : MonoBehaviour
    {
        public float2 TargetSize { get; set; }
        public int2 Position { get; set; }
        public char Letter { get; set; }
        public  bool IsEmpty { get; set; }
        public  bool IsFound { get; set; }

        public virtual void Configure(char letter, int2 position, float2 targetSize)
        {
            TargetSize = targetSize;
            Position = position;
            Letter = letter;
        }
        public abstract void AppearEmpty();
        public abstract void Found();
        public abstract void Disappear();
     
    }
}
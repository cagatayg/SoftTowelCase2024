using Unity.Mathematics;

namespace STGames
{
    public class EmptyLetterBox : BaseLetterBox
    {
        public override void Configure(char letter, int2 position, float2 targetSize)
        {
            IsEmpty = true;
            base.Configure(letter, position, targetSize);
        }
        public override void AppearEmpty() { }
        public override void Found() { }
        public override void Disappear() { }
    }
}
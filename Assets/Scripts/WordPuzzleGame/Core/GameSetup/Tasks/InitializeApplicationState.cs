using System.Collections;
using System.Threading.Tasks;

namespace STGames
{
    public class InitializeApplicationState : IGameTaskAsync
    {
        public float AllowedWaitTimeInSeconds { get; set; } = 4.9f;

        public Task Run()
        {
            AS.Language = LS.Get(SK.Language);
            return Task.FromResult(0);
        }
        
        public IEnumerator RunRoutine()
        {
            yield return null;
        }
    }
}

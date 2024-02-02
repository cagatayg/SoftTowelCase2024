using System.Collections;
using System.Threading.Tasks;

namespace STGames
{
    public interface IGameTaskAsync   
    {
        public float AllowedWaitTimeInSeconds { get; set; }
        Task Run();
        IEnumerator RunRoutine();
    }
}

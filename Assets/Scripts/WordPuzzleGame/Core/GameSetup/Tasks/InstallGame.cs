using System;
using System.Collections;
using System.Threading.Tasks;

namespace STGames
{
    public class InstallGame : IGameTaskAsync
    {
        public float AllowedWaitTimeInSeconds { get; set; } = 4.9f;

        public Task Run()
        {
            if (!LS.HasMilestoneReached(Milestone.GameInstalled))
            {
                LS.Milestone(Milestone.GameInstalled);
                LS.Set(SK.InstallationVersion, CurrentBuildNumber.Code);
                LS.Set(SK.InstallationDate, DateTime.Now);
                LS.Set(SK.GoldAmount, Config.InstallGoldAmount);
            }

            return Task.FromResult(0);
        }

        public IEnumerator RunRoutine()
        {
            yield return null;
        }
    }
}
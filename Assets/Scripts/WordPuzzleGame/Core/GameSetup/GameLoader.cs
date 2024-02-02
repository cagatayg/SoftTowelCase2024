using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace STGames
{
    public class GameLoader : Singleton<GameLoader>
    {
        private readonly List<IGameTaskAsync> _tasksConfig = new List<IGameTaskAsync>()
        {
            new InstallGame(),
            new InitializeApplicationState(),
        };

        private bool isFirstOpen = false;

        protected override void Awake()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            AS.HasGameLoaded = false;
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            if (!LS.HasMilestoneReached(Milestone.FirstLanguageSelected))
            {
                isFirstOpen = true;
                yield return new WaitForSeconds(0.35f);
                UINavigation.Instance.OpenPopup<LanguagePopupView>(outCallback: Restart);
                yield break;
            }

            foreach (var gameTaskRunner in _tasksConfig)
            {
                var task = gameTaskRunner.Run();
                yield return WaitForTask(task, gameTaskRunner.AllowedWaitTimeInSeconds);
                yield return gameTaskRunner.RunRoutine();
            }
            yield return null;
            AS.HasGameLoaded = true;
            if (isFirstOpen)
            {
                Navigation.ToScreen<GameplayView>();
            }
            else
            {
                yield return new WaitForSeconds(0.8f);
                Navigation.ToScreen<MainMenuView>();
            }
        }

        private void Restart()
        {
            StartCoroutine(Load());
        }
    }
}
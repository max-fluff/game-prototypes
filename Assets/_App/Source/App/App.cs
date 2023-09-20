using System.Linq;
using Omega.IoC;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class App : IStateSubject
    {
        private readonly StateMachine<App> _stateMachine;
        public IoContainer Services { get; private set; }

        public readonly AppRuntimeConfiguration RuntimeConfiguration;
        
        public App(AppRuntimeConfiguration runtimeConfiguration)
        {
            RuntimeConfiguration = runtimeConfiguration;
            _stateMachine = new StateMachine<App>(this);

            Application.targetFrameRate = runtimeConfiguration.LimitFps;

            InitUpdater();
            InitServices();
            Setup();
            InitState();
        }

        private void InitUpdater()
        {
            var appUpdate = new GameObject(nameof(AppUpdate), typeof(AppUpdate));
            appUpdate.GetComponent<AppUpdate>().App = this;
            Object.DontDestroyOnLoad(appUpdate);
        }

        private void InitServices()
        {
            var unityConfigs = UnityConfigs.Load();
            Services = IoContainer.Configure(c =>
            {
                c.AddSingleton(unityConfigs.Cursors);
                c.AddSingleton(unityConfigs.AppScenes);
                c.AddSingleton(unityConfigs.Environments);

                c.AddSingleton(RuntimeConfiguration);

                c.AddSingleton<Constants>();
                c.AddSingleton<AppSharedData>();
                c.AddSingleton<MouseInputService>();
                c.AddSingleton<SceneChangerService>();
                c.AddSingleton<QualitySettingsService>();
                c.AddSingleton<PlayerPrefsService>();
                c.AddSingleton<CursorService>();
                c.AddSingleton<KeyboardInputService>();
                c.AddSingleton<LocalizationService>();
            });
        }

        private void Setup()
        {
            var constants = Services.Resolve<Constants>();

            var playerPrefs = Services.Resolve<PlayerPrefsService>();
            var qualitySettings = Services.Resolve<QualitySettingsService>();
            qualitySettings.UpdateVSyncCount(playerPrefs.VSyncCount);

            Services.Resolve<CursorService>().SetDefaultCursor();
        }

        private void InitState()
        {
            // Для разработки, чтоб можно было запускать программу с любой сцены
            if (Application.isEditor)
            {
                var context = Object.FindObjectOfType<SceneContextBase>();
                _stateMachine.Launch(context switch
                {
                    StartScreenContext _ => new StartScreenState(),
                    WorkspaceContext _ => new WorkspaceState(),
                    _ => InitStateFromEnvironment()
                });
            }
            else
            {
                _stateMachine.Launch(new StartScreenState());
            }
        }

        private IAppState InitStateFromEnvironment()
        {
            var environmentContext = Object.FindObjectOfType<EnvironmentView>();

            if (environmentContext == null)
                return new EmptyState();

            if (Services.Resolve<Environments>().AllEnvironments.Any(env =>
                    env.Name == environmentContext.gameObject.scene.name))
            {
                Services.Resolve<AppSharedData>().Add(new EnvironmentSceneName
                {
                    Value = environmentContext.gameObject.scene.name
                });
                return new WorkspaceState();
            }

            Debug.LogError("The current scene is not added to EnvironmentScenes");
            return new EmptyState();
        }
        
        public void Run()
        {
            _stateMachine.Run();
        }

        public void OnApplicationQuit()
        {
        }
    }
}
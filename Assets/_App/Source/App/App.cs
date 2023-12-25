using Omega.IoC;
using UnityEngine;

namespace MaxFluff.Prototypes
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
                c.AddSingleton(unityConfigs.Games);

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
                c.AddSingleton<GravityService>();
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
            if (Application.isEditor)
            {
                var context = Object.FindObjectOfType<SceneContextBase>();
                _stateMachine.Launch(context switch
                {
                    StartScreenContext _ => new StartScreenState(),
                    PlatformerContext _ => new PlatformerState(),
                    PuzzleContext _ => new PuzzleState(),
                    RaceContext _ => new RaceState(),
                    DialogsContext _ => new DialogsState(),
                    FPSContext _ => new FPSState(),
                    TycoonContext _ => new TycoonState(),
                    NarrativeContext _ => new NarrativeState(),
                    TBSContext _ => new TBSState()
                });
            }
            else
            {
                _stateMachine.Launch(new StartScreenState());
            }
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
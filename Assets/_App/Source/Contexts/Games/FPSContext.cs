using MaxFluff.Prototypes.FPS;

namespace MaxFluff.Prototypes
{
    public class FPSContext : GameContext
    {
        public FPSPlayerView PlayerView;
        public CameraView MainCamera;
        public CoreView Core;
        public HealthVisualizationView HealthVisualization;
        public FailWindowView FailWindow;
        public WinWindowView WinWindow;
        public CoreVisualView CoreVisualView;
        public MobileRetranslatorPowerUpView MobileRetranslatorPowerUpView;
        public MobileRetranslatorVisualizerView MobileRetranslatorVisualizerView;
    }
}
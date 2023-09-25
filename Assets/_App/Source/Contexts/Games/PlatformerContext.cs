namespace MaxFluff.Prototypes
{
    public class PlatformerContext : GameContext
    {
        public PlatformerPlayerView PlayerView;
        public CameraView MainCameraView;
        public StateBasedGameObjectsControllerView StateBasedGameObjectsController;
        public StateSwitchAbilityTriggersListView StateSwitchAbilityTriggersList;
        public EnergyContainerView EnergyContainer;
        public ScoreCounterView ScoreCounterView;
        public EnergyCounterView EnergyCounterView;
    }
}
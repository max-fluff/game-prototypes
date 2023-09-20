using System;

namespace Omega.Kulibin
{
    public sealed class WorkspaceEvents
    {
        public event Action OnReset;
        public event Action OnSaveAdded;
        public event Action OnNewProgramCreated;
        public event Action OnSaveLoaded;
        public event Action OnProjectSaved;
        public event Action OnProjectSaveCancelled;
        public event Action OnProgramSavingRequested;
        public event Action OnProjectChanged;
        public event Action OnProjectGotChangeless;
        public event Action OnConfigurationChanged;
        public event Action OnLayersUpdateRequested;

        public void SendOnReset() => OnReset?.Invoke();
        public void SendOnNewProgramCreated() => OnNewProgramCreated?.Invoke();
        public void SendOnSaveAdded() => OnSaveAdded?.Invoke();
        public void SendOnSaveLoaded() => OnSaveLoaded?.Invoke();
        public void SendOnProjectSaved() => OnProjectSaved?.Invoke();
        public void SendOnProjectSaveCancelled() => OnProjectSaveCancelled?.Invoke();
        public void RequestProgramSaving() => OnProgramSavingRequested?.Invoke();
        public void SendOnProjectChanged() => OnProjectChanged?.Invoke();
        public void SendOnProjectGotChangeless() => OnProjectGotChangeless?.Invoke();
        public void SendOnConfigurationChanged() => OnConfigurationChanged?.Invoke();
        public void RequestLayersUpdate() => OnLayersUpdateRequested?.Invoke();
    }
}
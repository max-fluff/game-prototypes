using System;
using System.Collections.Generic;

namespace MaxFluff.Prototypes
{
    public sealed class AppCore
    {
        private readonly List<IInitBinding> _initBindings = new List<IInitBinding>();
        private readonly List<IRunBinding> _runBindings = new List<IRunBinding>();
        private readonly List<IDestroyBinding> _destroyBindings = new List<IDestroyBinding>();
        private readonly List<IStateChangerBinding> _stateChangerBindings = new List<IStateChangerBinding>();

        public event Action<IAppState> OnStateChangeRequested;

        public AppCore Add(IBinding binding)
        {
            if (binding is IInitBinding initBinding)
                _initBindings.Add(initBinding);
            
            if (binding is IRunBinding runBinding)
                _runBindings.Add(runBinding);
            
            if (binding is IDestroyBinding destroyBinding)
                _destroyBindings.Add(destroyBinding);
            
            if (binding is IStateChangerBinding stateChangerBinding)
            {
                _stateChangerBindings.Add(stateChangerBinding);
                stateChangerBinding.OnStateChangeRequested += RequestStateChange;
            }
            
            return this;
        }

        public void Init()
        {
            foreach (var binding in _initBindings)
                binding.Init();
        }

        public void Run()
        {
            foreach (var binding in _runBindings)
                binding.Run();
        }

        public void Destroy()
        {
            foreach (var binding in _destroyBindings)
                binding.Destroy();
        }

        private void RequestStateChange(IAppState state) => OnStateChangeRequested?.Invoke(state);
    }
}
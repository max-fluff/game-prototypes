using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class StateMachine<TSubject> where TSubject : IStateSubject
    {
        private readonly TSubject _subject;

        private IAsyncState<TSubject> _currentState;
        private bool _isStateInitialized;

        public StateMachine(TSubject subject)
        {
            _subject = subject;
        }

        private async UniTask SwitchStateAsync(IAsyncState<TSubject> nextState)
        {
            await Destroy();
            await InitializeState(nextState);
        }

        private async UniTask InitializeState(IAsyncState<TSubject> state)
        {
            try
            {
                await state.Init(_subject);
                _currentState = state;
                _currentState.OnStateChangeRequested += SwitchStateAsync;
                _isStateInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _isStateInitialized = false;
            }
        }

        public async UniTask Launch(IAsyncState<TSubject> firstState)
        {
            await SwitchStateAsync(firstState);
        }

        public async UniTask Run()
        {
            if (_isStateInitialized)
                await _currentState.Run(_subject);
        }

        public async UniTask Destroy()
        {
            if (_isStateInitialized)
            {
                _currentState.OnStateChangeRequested -= SwitchStateAsync;
                _isStateInitialized = false;
                await _currentState.Destroy(_subject);
            }
        }
    }
}
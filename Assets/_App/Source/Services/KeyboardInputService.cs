using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class KeyboardInputService : RunnableService
    {
        public event Action<Actions> OnInputAction;

        public bool IsKeyDown(KeyCode key) => 
            Input.GetKeyDown(key);

        protected override async UniTask Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                Actions action;

                if (Input.GetKey(KeyCode.LeftControl))
                    action = ProcessCtrlHotkeys();
                else
                    action = ProcessHotkeys();

                if (action != Actions.None)
                    OnInputAction?.Invoke(action);

                await UniTask.Yield(PlayerLoopTiming.PreUpdate);
            }
        }
        
        private static Actions ProcessHotkeys()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return Actions.Space;

            return Actions.None;
        }

        private static Actions ProcessCtrlHotkeys()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                return Actions.CtrlZ;

            if (Input.GetKeyDown(KeyCode.Y))
                return Actions.CtrlY;
            
            if (Input.GetKey(KeyCode.LeftShift))
                return ProcessCtrlShiftHotkeys();
            
            if (Input.GetKeyDown(KeyCode.O))
                return Actions.CtrlO;
            
            if (Input.GetKeyDown(KeyCode.S))
                return Actions.CtrlS;
            
            if (Input.GetKeyDown(KeyCode.N))
                return Actions.CtrlN;

            return Actions.None;
        }
        
        private static Actions ProcessCtrlShiftHotkeys()
        {
            if (Input.GetKeyDown(KeyCode.O))
                return Actions.CtrlShiftO;
            
            if (Input.GetKeyDown(KeyCode.S))
                return Actions.CtrlShiftS;

            return Actions.None;
        }
    }

    public enum Actions
    {
        CtrlZ,
        CtrlY,
        Space,
        CtrlO,
        CtrlS,
        CtrlN,
        CtrlShiftO,
        CtrlShiftS,
        None
    }
}
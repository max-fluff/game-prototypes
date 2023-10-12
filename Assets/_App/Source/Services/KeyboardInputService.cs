using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class KeyboardInputService : RunnableService
    {
        public event Action<Actions> OnInputAction;

        public bool IsKeyDown(KeyCode key) =>
            Input.GetKeyDown(key);

        public bool IsActionHeld(Actions key) =>
            key switch
            {
                Actions.Left => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow),
                Actions.Right => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow),
                Actions.Up => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow),
                Actions.Down => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow),
                Actions.Space => Input.GetKey(KeyCode.Space),
                Actions.R => Input.GetKey(KeyCode.R),
                _ => false
            };

        protected override async UniTask Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var action = ProcessHotkeys();

                if (action != Actions.None)
                    OnInputAction?.Invoke(action);

                await UniTask.Yield(PlayerLoopTiming.PreUpdate);
            }
        }

        private static Actions ProcessHotkeys()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return Actions.Space;

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                return Actions.Left;

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                return Actions.Right;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                return Actions.Up;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                return Actions.Down;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                return Actions.Selection1;

            if (Input.GetKeyDown(KeyCode.Alpha2))
                return Actions.Selection2;

            if (Input.GetKeyDown(KeyCode.Alpha3))
                return Actions.Selection3;

            if (Input.GetKeyDown(KeyCode.Escape))
                return Actions.Esc;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                return Actions.Shift;

            if (Input.GetKeyDown(KeyCode.R))
                return Actions.R;

            return Actions.None;
        }
    }

    public enum Actions
    {
        Left,
        Right,
        Up,
        Down,
        Space,
        Esc,
        Selection1,
        Selection2,
        Selection3,
        Shift,
        R,
        None
    }
}
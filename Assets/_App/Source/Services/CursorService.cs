using System.Linq;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class CursorService
    {
        private readonly CursorsConfig _cursorsConfig;
        private bool _isVisible;

        public bool IsCursorVisible
        {
            get => _isVisible;
            set
            {
                Cursor.visible = _isVisible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        public CursorService(CursorsConfig cursorsConfig)
        {
            _cursorsConfig = cursorsConfig;
        }

        public void SetCursorByCondition(CursorType cursorType, bool condition)
        {
            if (condition)
                SetCursor(cursorType);
            else
                SetDefaultCursor();
        }

        public void SetDefaultCursor()
        {
            SetCursor(CursorType.Default);
        }

        public void SetCursor(CursorType cursorType)
        {
            try
            {
                var requiredCursor = _cursorsConfig.Cursors.First(cursor => cursor.Type == cursorType);
                Cursor.SetCursor(
                    requiredCursor.Texture,
                    requiredCursor.Pivot,
                    CursorMode.Auto);
            }
            catch
            {
                Cursor.SetCursor(
                    null,
                    Vector2.zero,
                    CursorMode.Auto);
            }
        }
    }
}
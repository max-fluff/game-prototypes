using System.Linq;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class CursorService
    {
        private readonly CursorsConfig _cursorsConfig;

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
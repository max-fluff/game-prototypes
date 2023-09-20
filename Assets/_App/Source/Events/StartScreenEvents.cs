using System;

namespace Omega.Kulibin
{
    public sealed class StartScreenEvents
    {
        public event Action OnOpenPolygon;
        public event Action OnOpenConstructor;
        

        public void OpenPolygon() => OnOpenPolygon?.Invoke();
        public void OpenConstructor() => OnOpenConstructor?.Invoke();
    }
}
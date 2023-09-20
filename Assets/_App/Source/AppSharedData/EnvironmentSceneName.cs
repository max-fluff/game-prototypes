using System;

namespace Omega.Kulibin
{
    [Serializable]
    public sealed class EnvironmentSceneName : ISharedData
    {
        public string Value;

        public EnvironmentSceneName()
        {
        }

        public EnvironmentSceneName(string value)
        {
            Value = value;
        }
    }
}
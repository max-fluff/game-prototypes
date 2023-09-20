using System;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Video;

namespace Omega.Kulibin
{
    [CreateAssetMenu(fileName = "Environments", menuName = "Config/Environments", order = 0)]
    public sealed class Environments : ScriptableObject
    {
        public EnvironmentData CalibrationEnvironment;
        public EnvironmentData MarsEnvironment;
        public EnvironmentData CustomEnvironment;
        public SceneReference[] AllEnvironments;
    }

    [Serializable]
    public struct EnvironmentData
    {
        public SceneReference Scene;
        public VideoClip PreviewVideo;
    }
}
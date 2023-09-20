using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Omega.Kulibin
{
    public enum SizeMode
    {
        Absolute,
        Relative,
    }
    
    [Serializable]
    public struct AdjustableSize
    {
        [SerializeField]
        public SizeMode Mode;

        [SerializeField, Range(0f, 1f), ShowIf(nameof(Mode), SizeMode.Relative)]
        public float Relative;

        [SerializeField, ShowIf(nameof(Mode), SizeMode.Absolute)]
        public float Absolute;

        public float Get(float relativeTarget)
        {
            return Mode switch
            {
                SizeMode.Absolute => Absolute,
                SizeMode.Relative => Relative * relativeTarget,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Set(float value, float relativeTarget)
        {
            Absolute = value;
            Relative = value / relativeTarget;
        }
    }
}
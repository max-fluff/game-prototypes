using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class GravityService
    {
        public readonly Vector3 DefaultGravity;

        public GravityService()
        {
            DefaultGravity = Physics.gravity;
        }

        public float GravityPower
        {
            get => Physics.gravity.magnitude;
            set => Physics.gravity = Physics.gravity.normalized * value;
        }

        public bool SetGravityDirection(Vector3 gravity)
        {
            var notEqual = Physics.gravity.normalized != gravity.normalized;
            if (notEqual)
                Physics.gravity = gravity.normalized * Physics.gravity.magnitude;

            return notEqual;
        }

        public void SetGravity(Vector3 gravity)
        {
            if (Physics.gravity != gravity)
                Physics.gravity = gravity;
        }

        public void ResetGravity() =>
            Physics.gravity = DefaultGravity;
    }
}
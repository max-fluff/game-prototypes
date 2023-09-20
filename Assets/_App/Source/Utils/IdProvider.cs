using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class IdProvider : MonoBehaviour
    {
        private static int _nextId;

        public int Id;

        public void Awake() => Id = _nextId++;

        public static implicit operator int(IdProvider id)
        {
            return id.Id;
        }
    }
}
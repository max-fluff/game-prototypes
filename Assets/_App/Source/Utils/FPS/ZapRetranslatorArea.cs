using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes.FPS
{
    public class ZapRetranslatorArea : MonoBehaviour
    {
        public List<IZappableObject> ZappableObjectsInRange = new List<IZappableObject>();

        private void OnTriggerEnter(Collider other)
        {
            var zappable = other.GetComponent<IZappableObject>();
            if (zappable != null)
                ZappableObjectsInRange.Add(zappable);
        }

        private void OnTriggerExit(Collider other)
        {
            var zappable = other.GetComponent<IZappableObject>();
            if (zappable != null && ZappableObjectsInRange.Contains(zappable))
                ZappableObjectsInRange.Remove(zappable);
        }
    }
}
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RemoteWallPanel : MonoBehaviour, IZappableObject
    {
        public FPSRemoteSender RemoteSender;

        public void Zap()
        {
            RemoteSender.Activate();
        }
    }
}
using TMPro;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class AppVersionDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        private void Awake()
        {
            label.text = Application.version;
        }
    }
}
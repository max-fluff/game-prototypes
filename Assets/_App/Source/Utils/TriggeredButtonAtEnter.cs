using Lean.Gui;
using UnityEngine;

namespace Omega.Kulibin
{
    [RequireComponent(typeof(LeanButton))]
    public class TriggeredButtonAtEnter : MonoBehaviour
    {
        [SerializeField] private LeanButton Button;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                Button.OnClick.Invoke();
        }
    }
}
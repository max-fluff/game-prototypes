using I2.Loc;
using Lean.Gui;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class LocalizedLeanTooltipData : LeanTooltipData
    {
        [SerializeField] private LocalizedString terms;
        public override string Text => terms;
    }
}
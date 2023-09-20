using TMPro;
using UnityEngine;

namespace Omega.Kulibin
{
    [CreateAssetMenu(fileName = "VariableNameValidator", menuName = "Validators/VariableNameValidator", order = 0)]
    public sealed class VariableNameValidator : TMP_InputValidator
    {
        [SerializeField] private int characterLimit = 32;
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (text.Length >= characterLimit)
                return (char) 0;
            if (ch == '\t' || ch == '#')
                return (char) 0;
            if (pos == 0)
            {
                if (ch == ' ')
                    return (char) 0;
            }
            else
            {
                if (text[pos - 1] == ' ' && ch == ' ')
                    return (char) 0;
            }

            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
    }
}
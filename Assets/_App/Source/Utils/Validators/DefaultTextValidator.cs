using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Omega.Kulibin
{
    [CreateAssetMenu(fileName = "DefaultTextValidator", menuName = "Validators/DefaultTextValidator", order = 0)]
    public sealed class DefaultTextValidator : TMP_InputValidator
    {
        [SerializeField] private int characterLimit = 32;
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (text.Length >= characterLimit)
                return (char) 0;
            
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
    }
}
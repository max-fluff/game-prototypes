using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Omega.Kulibin
{
    public class InputFieldValueClamp : MonoBehaviour
    {
        public InputField InputField;

        [ShowIf(nameof(IntegerNumber))] public int MinIntValue;
        [ShowIf(nameof(IntegerNumber))] public int MaxIntValue;
        
        [ShowIf(nameof(DecimalNumber))] public float MinFloatValue;
        [ShowIf(nameof(DecimalNumber))] public float MaxFloatValue;

        private bool IntegerNumber => InputField != null &&
                                      InputField.contentType == InputField.ContentType.IntegerNumber;

        private bool DecimalNumber => InputField != null &&
                                      InputField.contentType == InputField.ContentType.DecimalNumber;

        private void OnEnable()
        {
            if (IntegerNumber)
                InputField.onValueChanged.AddListener(ClampInt);
            else if (DecimalNumber) 
                InputField.onValueChanged.AddListener(ClampFloat);
        }
        
        private void OnDisable()
        {
            if (IntegerNumber)
                InputField.onValueChanged.RemoveListener(ClampInt);
            else if (DecimalNumber) 
                InputField.onValueChanged.RemoveListener(ClampFloat);
        }

        private void ClampInt(string valueRaw)
        {
            if (!int.TryParse(valueRaw, out var value) ||
                value >= MinIntValue && value <= MaxIntValue)
                return;

            value = Mathf.Clamp(value, MinIntValue, MaxIntValue);
            InputField.text = value.ToString();
        }

        private void ClampFloat(string valueRaw)
        {
            if (!float.TryParse(valueRaw, out var value) ||
                value >= MinFloatValue && value <= MaxFloatValue)
                return;
            
            value = Mathf.Clamp(value, MinFloatValue, MaxFloatValue);
            InputField.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
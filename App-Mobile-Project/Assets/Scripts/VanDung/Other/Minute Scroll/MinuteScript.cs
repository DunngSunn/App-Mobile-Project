using TMPro;
using UnityEngine;

namespace UI
{
    public class MinuteScript : MonoBehaviour
    {
        #region Fields

        public TextMeshProUGUI minuteText;
        
        public int Minute { get; private set; }

        #endregion

        private void Reset()
        {
            minuteText = GetComponent<TextMeshProUGUI>();
        }

        public void SetMinute(int minuteToSet)
        {
            minuteText.text = minuteToSet <= 9 ? $"0{minuteToSet}" : minuteToSet.ToString("##");
            Minute = minuteToSet;
        }
    }
}
using TMPro;
using UnityEngine;

namespace UI.Hour_Scroll
{
    public class HourScript : MonoBehaviour
    {
        #region Fields

        public TextMeshProUGUI hourText;
        
        public int Hour { get; private set; }

        #endregion

        private void Reset()
        {
            hourText = GetComponent<TextMeshProUGUI>();
        }

        public void SetHour(int hourToSet)
        {
            hourText.text = hourToSet <= 9 ? $"0{hourToSet}" : hourToSet.ToString("##");
            Hour = hourToSet;
        }
    }
}
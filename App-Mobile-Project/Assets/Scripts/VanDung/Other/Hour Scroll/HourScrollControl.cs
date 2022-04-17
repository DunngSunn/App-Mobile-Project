using UnityEngine;
using UnityEngine.UI.Extensions;

namespace UI.Hour_Scroll
{
    public class HourScrollControl : MonoBehaviour
    {
        #region Fields

        public VerticalScrollSnap verticalScroll;

        public HourScript[] allHourScripts;

        public HourScript CurrentHour { get; private set; }

        #endregion

        public void InitForSleep()
        {
            for (var i = 0; i < allHourScripts.Length; i++)
            {
                allHourScripts[i].SetHour(i);
            }
            
            CurrentHour = allHourScripts[verticalScroll.StartingScreen];
        }
        
        public void OnSelectionPageChangedEvent()
        {
            CurrentHour = allHourScripts[verticalScroll.CurrentPage];
        }
    }
}
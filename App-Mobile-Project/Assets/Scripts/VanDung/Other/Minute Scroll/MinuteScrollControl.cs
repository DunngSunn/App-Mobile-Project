using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace UI
{
    public class MinuteScrollControl : MonoBehaviour
    {
        #region Fields

        public VerticalScrollSnap verticalScroll;

        public MinuteScript[] allMinuteScripts;

        public MinuteScript CurrentMinute { get; private set; }

        #endregion

        public void InitForFocus()
        {
            var startMinute = 0;
            for (var i = 0; i < allMinuteScripts.Length; i++)
            {
                if (i < 12)
                    startMinute += 5;
                else
                    startMinute += 10;

                allMinuteScripts[i].SetMinute(startMinute);
            }

            CurrentMinute = allMinuteScripts[verticalScroll.StartingScreen];
        }

        public void InitForSleep()
        {
            for (var i = 0; i < allMinuteScripts.Length; i++)
            {
                allMinuteScripts[i].SetMinute(i);
            }

            CurrentMinute = allMinuteScripts[verticalScroll.StartingScreen];
        }
        
        public void OnSelectionPageChangedEvent()
        {
            CurrentMinute = allMinuteScripts[verticalScroll.CurrentPage];
        }
    }
}
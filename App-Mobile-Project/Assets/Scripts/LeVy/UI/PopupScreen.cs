using System;
using Coffee.UIExtensions;
using DunnGSunn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupScreen : SunUI
    {
        #region Fields

        [Header("Text")]
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI totalTimeText;

        [Header("Button")]
        public Button closeButton;

        [Header("FX")]
        public UIParticle particle;
        
        #endregion

        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện cho các nút
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        public override void Show()
        {
            base.Show();
            
            // Rung và bật fx
            Handheld.Vibrate();
            particle.Play();
        }

        public override void Hide()
        {
            base.Hide();
            
            // Tắt fx
            particle.Stop();
        }

        #endregion

        private void OnCloseButtonClick()
        {
            // Về màn hình trước đó
            SunUIController.PopScreen();
        }

        public void SetText(string mode, TimeSpan time, TimeSpan totalTime)
        {
            // Hiển thị text ra màn hình
            timeText.text = $"Bạn đã hoàn thành {time.Minutes} phút {time.Seconds} giây trong lần {mode} này.";
            totalTimeText.text = $"Tổng thời gian bạn sử dụng chức năng này là {totalTime.Minutes} phút {totalTime.Seconds} giây.";
        }
    }
}
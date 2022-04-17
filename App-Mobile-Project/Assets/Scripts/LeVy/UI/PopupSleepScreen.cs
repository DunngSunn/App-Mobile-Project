using System;
using Coffee.UIExtensions;
using DunnGSunn;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupSleepScreen : SunUI
    {
        #region Fields
        
        // Thời gian giữa các lần rung
        private const float TimeVibrate = 5f;

        [Header("Text")]
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI quoteText;
        
        [Header("Button")]
        public Button closeButton;

        [Header("FX")]
        public UIParticle particle;

        private bool _vibrate;
        private float _currentTimeVibrate;
        private TimeSpan _currentTimeSpan;
        
        #endregion

        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện cho nút
            closeButton.onClick.AddListener(OnCloseButtonClick);

            // Hiển thị quote
            var splitQuote = QuoteManager.Instance.GetQuote().Split('*');
            quoteText.text = $"{splitQuote[0]} \n {splitQuote[1]}";
        }

        public override void Show()
        {
            base.Show();
            
            // Rung và bật fx
            Handheld.Vibrate();
            particle.Play();
            
            // Bật nhạc báo thức
            AudioManager.Instance.PlaySound("Alarm");
            
            // Set rung
            _currentTimeVibrate = TimeVibrate;
            _vibrate = true;

            // Hiển thị thời gian hiện tại
            _currentTimeSpan = DateTime.Now.TimeOfDay;
            timeText.text = $"{_currentTimeSpan.Hours:00} : {_currentTimeSpan.Minutes:00}";
        }

        public override void Hide()
        {
            base.Hide();
            
            // Tắt nhạc và fx
            particle.Stop();
            AudioManager.Instance.StopSound("Alarm");
        }

        #endregion
        
        private void Update()
        {
            // Kiểm tra nếu cho phép rung thì mỗi mấy giây sẽ rung 1 lần
            if (_vibrate)
            {
                _currentTimeVibrate -= Time.deltaTime;
                if (_currentTimeVibrate <= 0f)
                {
                    _currentTimeVibrate = TimeVibrate;
                    Handheld.Vibrate();
                }
            }
            
            // Hiển thị thời gian hiện tại
            _currentTimeSpan = DateTime.Now.TimeOfDay;
            timeText.text = $"{_currentTimeSpan.Hours:00} : {_currentTimeSpan.Minutes:00}";
        }

        private void OnCloseButtonClick()
        {
            // Tắt rung và về màn hình chính
            _vibrate = false;
            SunUIController.PopScreen();
        }
    }
}
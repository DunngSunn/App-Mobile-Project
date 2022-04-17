using System;
using DunnGSunn;
using TMPro;
using UI.Hour_Scroll;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SleepScreen : SunUI
    {
        #region Fields

        [Header("Button")]
        public Button closeButton;
        
        [Header("Choose timer")]
        public GameObject chooseTimerPanel;
        public SunTween tweenChooseTimerPanel;
        public HourScrollControl hourScrollControl;
        public MinuteScrollControl minuteScrollControl;
        public Button startButton;
        
        [Header("Start timer")] 
        public GameObject startTimerPanel;
        public SunTween tweenStartTimerPanel;
        public SunTween tweenTwoDot;
        public TextMeshProUGUI hourText;
        public TextMeshProUGUI minuteText;
        public Button stopButton;
        public SunTween tweenCircle;

        private bool _isShowChooseTimer;
        private float _currentFloatSecond;
        private TimeSpan _currentTime;
        
        #endregion

        #region Unity callback functions

        public override void Initialize()
        {
            // Thêm sự kiện cho các nút
            closeButton.onClick.AddListener(OnCloseButtonClick);
            startButton.onClick.AddListener(OnStartButtonClick);
            stopButton.onClick.AddListener(OnStopButtonClick);
            
            // Setup cho scroll phút và giờ
            hourScrollControl.InitForSleep();
            minuteScrollControl.InitForSleep();
            
            // Set hiển thị chọn thời gian lên trước
            _isShowChooseTimer = true;
            
            // Nghe sự kiện
            SunEventManager.StartListening(EventID.SleepStop, OnSleepStop);
        }
        
        private void OnEnable()
        {
            // Kiểm tra hiển thị chọn hay là đếm thời gian
            if (_isShowChooseTimer)
            {
                chooseTimerPanel.SetActive(true);
                startTimerPanel.SetActive(false);
            }
            else
            {
                chooseTimerPanel.SetActive(false);
                startTimerPanel.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            // Dừng nghe sự kiện
            SunEventManager.StopListening(EventID.SleepStop, OnSleepStop);
        }

        #endregion

        #region Button events

        private void OnCloseButtonClick()
        {
            if (!CanClick) return;
            
            // Về màn hình chính
            SunUIController.PopScreen();
        }
        
        private void OnStartButtonClick()
        {
            // Hiển thị màn hình đếm thời gian
            _isShowChooseTimer = false;
            tweenChooseTimerPanel.PlayReverse();
            tweenStartTimerPanel.PlayForward();
            tweenTwoDot.PlayForward();
            
            // Hiển thị thời gian đích
            hourText.text = $"{hourScrollControl.CurrentHour.Hour:00}";
            minuteText.text = $"{minuteScrollControl.CurrentMinute.Minute:00}";
            
            // Chạy animation của vòng tròn bên ngoài
            tweenCircle.PlayForward();

            // Bắn sự kiện mode báo thức được bắt đầu 
            SunEventManager.EmitEvent(EventID.SleepStart, sender: TimeSpan.FromMinutes(hourScrollControl.CurrentHour.Hour * 60 + minuteScrollControl.CurrentMinute.Minute));
        }
        
        private void OnStopButtonClick()
        {
            // Bắn sự kiện mode báo thức kết thúc
            SunEventManager.EmitEvent(EventID.SleepStop);
        }
        
        #endregion

        #region Mode

        private void OnSleepStop()
        {
            // Hiển thị chọn thời gian và dừng animation của vòng tròn
            _isShowChooseTimer = true;
            tweenChooseTimerPanel.PlayForward();
            tweenStartTimerPanel.PlayReverse();
            tweenCircle.Stop(true);
        }

        #endregion
    }
}
using System;
using DunnGSunn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FocusScreen : SunUI
    {
        #region Fields

        [Header("Button")] 
        public Button closeButton;
        
        [Header("Choose timer")]
        public GameObject chooseTimerPanel;
        public SunTween tweenChooseTimerPanel;
        public MinuteScrollControl timerModeMinuteScroll;
        public Button startButton;

        [Header("Start timer")] 
        public GameObject startTimerPanel;
        public SunTween tweenStartTimerPanel;
        public TextMeshProUGUI timerText;
        public Button stopButton;
        public SunTween tweenCircle;
        
        private bool _isShowChooseTimer;
        private float _currentFloatSecond;
        private TimeSpan _currentTime;

        #endregion

        #region Unity callback functions
        
        public override void Initialize()
        {
            // Thêm sự kiện vào các nút trong màn hình
            closeButton.onClick.AddListener(OnCloseButtonClick);
            startButton.onClick.AddListener(OnStartButtonClick);
            stopButton.onClick.AddListener(OnStopButtonClick);
            
            // Setup cho scroll phút
            timerModeMinuteScroll.InitForFocus();
            
            // Set hiển thị chọn phút lên trước
            _isShowChooseTimer = true;

            // Nghe sự kiện
            SunEventManager.StartListening(EventID.FocusUpdate, OnFocusUpdate);
            SunEventManager.StartListening(EventID.FocusStop, OnFocusStop);
        }

        private void OnEnable()
        {
            // Kiểm tra hiển thị chọn phút hay là đếm thời gian
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
            SunEventManager.StopListening(EventID.FocusUpdate, OnFocusUpdate);
            SunEventManager.StopListening(EventID.FocusStop, OnFocusStop);
        }

        #endregion

        #region Button events

        // 
        private void OnCloseButtonClick()
        {
            if (!CanClick) return;
            
            // Về màn hình chính
            SunUIController.PopScreen();
        }
        
        //
        private void OnStartButtonClick()
        {
            // Hiển thị màn hình đếm thời gian
            _isShowChooseTimer = false;
            tweenChooseTimerPanel.PlayReverse();
            tweenStartTimerPanel.PlayForward();
            
            // Set thời gian của lần tập trung này và hiển thị lên
            _currentTime = TimeSpan.FromSeconds(timerModeMinuteScroll.CurrentMinute.Minute * 60);
            timerText.text = $"{_currentTime.Minutes:00} : {_currentTime.Seconds:00}";
            
            // Chạy animation của vòng tròn bên ngoài
            tweenCircle.PlayForward();
            
            // Bắn sự kiện mode tập trung được bắt đầu 
            SunEventManager.EmitEvent(EventID.FocusStart, sender: timerModeMinuteScroll.CurrentMinute.Minute * 60);
        }
        
        // 
        private void OnStopButtonClick()
        {
            // Bắn sự kiện mode tập trung kết thúc
            SunEventManager.EmitEvent(EventID.FocusStop);
        }
        
        #endregion

        #region Mode

        private void OnFocusUpdate()
        {
            // Nhận thời gian còn lại của mode
            _currentFloatSecond = (float)SunEventManager.GetSender(EventID.FocusUpdate);
            
            // Hiển thị thời gian còn lại
            _currentTime = TimeSpan.FromSeconds(_currentFloatSecond);
            timerText.text = $"{_currentTime.Minutes:00} : {_currentTime.Seconds:00}";
        }
        
        private void OnFocusStop()
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
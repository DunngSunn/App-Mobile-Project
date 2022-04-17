using System;
using DunnGSunn;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainScreen : SunUI
    {
        #region Fields

        [Header("Quote text")]
        public TextMeshProUGUI quoteText;

        [Header("Mode")] 
        public TextMeshProUGUI modeText;

        [Header("Button")] 
        public Button focusButton;
        public Button sleepButton;
        public Button breathButton;
        public Button napButton;
        public Button homeButton;
        public Button accountButton;
        
        [Header("Date/Time")]
        public TextMeshProUGUI dateTimeText;

        [Header("Tween")] 
        public SunTween tweenHome;
        public SunTween tweenAccount;

        private DateTime _currentDateTime;

        private bool _isShowHome;
        private bool _isShowAccount;
        
        #endregion

        public override void Initialize()
        {
            // Hiển thị quote
            var splitQuote = QuoteManager.Instance.GetQuote().Split('*');
            quoteText.text = $"{splitQuote[0]} \n {splitQuote[1]}";

            // Thêm sự kiện cho các button
            focusButton.onClick.AddListener(OnFocusButtonClick);
            sleepButton.onClick.AddListener(OnSleepButtonClick);
            breathButton.onClick.AddListener(OnBreathButtonClick);
            napButton.onClick.AddListener(OnNapButtonClick);
            homeButton.onClick.AddListener(OnHomeButtonClick);
            accountButton.onClick.AddListener(OnAccountButtonClick);
            
            // Hiển thị thời gian hiện tại
            _currentDateTime = DateTime.UtcNow.ToLocalTime();
            dateTimeText.text = $"{_currentDateTime:dd/MM/yyyy}\n{_currentDateTime:hh:mm}";
            
            // Hiển thị home trước
            _isShowHome = true;
            tweenHome.SetEndToCurrentValue();
            tweenAccount.SetStartToCurrentValue();

            // Tắt account
            tweenAccount.gameObject.SetActive(false);
        }

        
        public override void Show()
        {
            base.Show();

            // Kiểm tra xem có đang ở mode nào không, nếu có thì hiển thị mode đó lên màn hình
            switch (AppManager.Instance.CurrentMode)
            {
                case Mode.None:
                    modeText.text = string.Empty;
                    break;
                case Mode.Focus:
                    modeText.text = "Chế độ tập trung";
                    break;
                case Mode.Sleep:
                    modeText.text = "Chế độ báo thức";
                    break;
                case Mode.Breath:
                    modeText.text = "Chế độ hít thở";
                    break;
            }
        }

        #region Button events

        private void OnFocusButtonClick()
        {
            if (!CanClick) return;
            // Kiểm tra có đang ở các mode khác không
            if (AppManager.Instance.CurrentMode == Mode.Breath || AppManager.Instance.CurrentMode == Mode.Sleep) return;

            // Hiển thị màn hình tập trung
            SunUIController.PushScreen<FocusScreen>(hideCurrentScreen: true);
        }
        
        private void OnSleepButtonClick()
        {
            if (!CanClick) return;
            // Kiểm tra có đang ở các mode khác không
            if (AppManager.Instance.CurrentMode == Mode.Breath || AppManager.Instance.CurrentMode == Mode.Focus) return;

            // Hiển thị màn hình báo thức
            SunUIController.PushScreen<SleepScreen>(hideCurrentScreen: true);
        }
        
        private void OnBreathButtonClick()
        {
            if (!CanClick) return;
            // Kiểm tra có đang ở các mode khác không
            if (AppManager.Instance.CurrentMode == Mode.Focus || AppManager.Instance.CurrentMode == Mode.Sleep) return;
            
            // Hiển thị màn hình hít thở
            SunUIController.PushScreen<BreathScreen>(hideCurrentScreen: true);
        }
        
        private void OnNapButtonClick()
        {
            if (!CanClick) return;
        }
        
        private void OnHomeButtonClick()
        {
            if (!CanClick) return;

            // Kiểm tra có đang ở màn hình home không
            if (_isShowHome) return;

            // Hiển thị màn hình home
            _isShowHome = true;
            _isShowAccount = false;
            tweenHome.PlayForward();
            tweenAccount.PlayReverse();
        }
        
        private void OnAccountButtonClick()
        {
            if (!CanClick) return;

            // Kiểm tra có đang ở màn hình account không
            if (_isShowAccount) return;

            // Hiển thị màn hình account
            _isShowHome = false;
            _isShowAccount = true;
            tweenHome.PlayReverse();
            tweenAccount.PlayForward();
        }

        #endregion

        #region Unity callback functions

        private void FixedUpdate()
        {
            // Update thời gian hiện tại và hiển thị lên
            _currentDateTime = DateTime.UtcNow.ToLocalTime();
            dateTimeText.text = $"{_currentDateTime:dd/MM/yyyy}\n{_currentDateTime:hh:mm}";
        }

        #endregion
    }
}
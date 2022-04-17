using DunnGSunn;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BreathScreen : SunUI
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
        public TextMeshProUGUI breathText;
        public Button stopButton;
        public SunTween tweenCircle;
        
        private bool _isShowChooseTimer;
        private float _currentFloatSecond;

        // Const
        private const float SecondCycleInBreathMode = 5.5f;

        #endregion
        
        #region Unity callback functions
        
        public override void Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
            startButton.onClick.AddListener(OnStartButtonClick);
            stopButton.onClick.AddListener(OnStopButtonClick);
            
            timerModeMinuteScroll.InitForFocus();
            
            _isShowChooseTimer = true;

            SunEventManager.StartListening(EventID.BreathChangeCycle, OnBreathChangeCycle);
            SunEventManager.StartListening(EventID.BreathUpdate, OnBreathUpdate);
            SunEventManager.StartListening(EventID.BreathStop, OnBreathStop);
        }

        private void OnEnable()
        {
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
            SunEventManager.StopListening(EventID.BreathUpdate, OnBreathUpdate);
            SunEventManager.StopListening(EventID.BreathStop, OnBreathStop);
        }

        #endregion
        
        #region Button events

        private void OnCloseButtonClick()
        {
            if (!CanClick) return;
            
            SunUIController.PopScreen();
        }
        
        private void OnStartButtonClick()
        {
            _isShowChooseTimer = false;
            tweenChooseTimerPanel.PlayReverse();
            tweenStartTimerPanel.PlayForward();
            
            timerText.text = $"{Mathf.RoundToInt(SecondCycleInBreathMode)}";
            breathText.text = "Hít vào";

            tweenCircle.PlayForward();
            
            SunEventManager.EmitEvent(EventID.BreathStart, sender: timerModeMinuteScroll.CurrentMinute.Minute * 60);
            SunEventManager.EmitEvent(EventID.BreathInAudio);
        }
        
        private void OnStopButtonClick()
        {
            SunEventManager.EmitEvent(EventID.BreathStop);
        }
        
        #endregion
        
        #region Mode

        private void OnBreathChangeCycle()
        {
            var sender = (CycleBreathMode) SunEventManager.GetSender(EventID.BreathChangeCycle);
            if (sender == CycleBreathMode.BreathIn)
            {
                breathText.text = "Hít vào";
                tweenCircle.Stop();
                tweenCircle.PlayForward();
                Handheld.Vibrate();
                SunEventManager.EmitEvent(EventID.BreathInAudio);
            }
            else
            {
                breathText.text = "Thở ra";
                tweenCircle.Stop();
                tweenCircle.PlayReverse();
                Handheld.Vibrate();
                SunEventManager.EmitEvent(EventID.BreathOutAudio);
            }
        }

        private void OnBreathUpdate()
        {
            var sender = (int) SunEventManager.GetSender(EventID.BreathUpdate);
            timerText.text = $"{sender}";
        }
        
        private void OnBreathStop()
        {
            _isShowChooseTimer = true;
            tweenChooseTimerPanel.PlayForward();
            tweenStartTimerPanel.PlayReverse();
        }

        #endregion
    }
}
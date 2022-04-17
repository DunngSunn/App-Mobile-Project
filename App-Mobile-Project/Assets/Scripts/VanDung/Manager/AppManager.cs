using System;
using DunnGSunn;
using UI;
using UnityEngine;

namespace Manager
{
    //
    public enum Mode
    {
        None,
        Focus,
        Sleep,
        Breath
    }

    //
    public enum CycleBreathMode
    {
        BreathOut,
        BreathIn
    }

    public class AppManager : SunMonoSingleton<AppManager>
    {
        #region Fields

        [Header("Speed delta time")] 
        [Range(1f, 100f)] public float speedDeltaTime = 1f;

        // Focus mode
        private bool _isStartFocusMode;
        //
        private int _totalSecondInFocus;
        private int _currentFocusIntSecond;
        private float _currentFocusFloatSecond;
        
        // Sleep mode
        private bool _isStartSleepMode;
        //
        private TimeSpan _endSleepTimeSpan;
        private TimeSpan _nowSleepTimeSpan;
        
        // Breath mode
        private bool _isStartBreathMode;
        //
        private int _totalSecondInBreath;
        private int _currentBreathIntSecond;
        private int _currentCycleBreathIntSecond;
        private float _currentCycleBreathFloatSecond;
        private CycleBreathMode _cycleBreathMode;

        // Const
        private const float SecondCycleInBreathMode = 5.5f;
        
        //
        public Mode CurrentMode { get; set; }

        //
        public static int TotalSecondFocusMode
        {
            get => PlayerPrefs.GetInt("TotalSecondFocusMode", 0);
            set => PlayerPrefs.SetInt("TotalSecondFocusMode", value);
        }

        //
        public static int TotalSecondBreathMode
        {
            get => PlayerPrefs.GetInt("TotalSecondBreathMode", 0);
            set => PlayerPrefs.SetInt("TotalSecondBreathMode", value);
        }

        #endregion

        #region Unity callback functions

        protected override void LoadInAwake()
        {
            CurrentMode = Mode.None;
            
            // Focus mode
            SunEventManager.StartListening(EventID.FocusStart, OnFocusTimerModeStart);
            SunEventManager.StartListening(EventID.FocusStop, OnFocusTimerModeStop);
            
            // Sleep mode
            SunEventManager.StartListening(EventID.SleepStart, OnSleepModeStart);
            SunEventManager.StartListening(EventID.SleepStop, OnSleepModeStop);
            
            // Breath mode
            SunEventManager.StartListening(EventID.BreathStart, OnBreathModeStart);
            SunEventManager.StartListening(EventID.BreathStop, OnBreathModeStop);
            
             Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnDestroy()
        {
            // Focus mode
            SunEventManager.StopListening(EventID.FocusStart, OnFocusTimerModeStart);
            SunEventManager.StopListening(EventID.FocusStop, OnFocusTimerModeStop);
            
            // Sleep mode
            SunEventManager.StopListening(EventID.SleepStart, OnSleepModeStart);
            SunEventManager.StopListening(EventID.SleepStop, OnSleepModeStop);
            
            // Breath mode
            SunEventManager.StopListening(EventID.BreathStart, OnBreathModeStart);
            SunEventManager.StopListening(EventID.BreathStop, OnBreathModeStop);
        }
        
        private void Update()
        {
            // Focus mode
            if (_isStartFocusMode)
            {
                _currentFocusFloatSecond -= Time.deltaTime * speedDeltaTime;
                if (_currentFocusIntSecond != Mathf.RoundToInt(_currentFocusFloatSecond))
                {
                    _currentFocusIntSecond = Mathf.RoundToInt(_currentFocusFloatSecond);
                    SunEventManager.EmitEvent(EventID.FocusUpdate, sender: _currentFocusFloatSecond);
                }

                if (_currentFocusFloatSecond <= 0)
                {
                    _isStartFocusMode = false;
                    SunEventManager.EmitEvent(EventID.FocusStop);
                }
            }
            
            // Sleep mode
            if (_isStartSleepMode)
            {
                _nowSleepTimeSpan = DateTime.UtcNow.ToLocalTime().TimeOfDay;
                if (_nowSleepTimeSpan.Hours == _endSleepTimeSpan.Hours && _nowSleepTimeSpan.Minutes == _endSleepTimeSpan.Minutes)
                {
                    _isStartSleepMode = false;
                    SunEventManager.EmitEvent(EventID.SleepStop);
                }
            }
            
            // Breath mode
            if (_isStartBreathMode)
            {
                _currentCycleBreathFloatSecond -= Time.deltaTime * speedDeltaTime;
                if (_currentCycleBreathIntSecond != Mathf.RoundToInt(_currentCycleBreathFloatSecond))
                {
                    _currentCycleBreathIntSecond = Mathf.RoundToInt(_currentCycleBreathFloatSecond);
                    _currentBreathIntSecond++;
                    if (_currentCycleBreathIntSecond <= 0)
                    {
                        _currentCycleBreathFloatSecond = SecondCycleInBreathMode;
                        _currentCycleBreathIntSecond = Mathf.RoundToInt(SecondCycleInBreathMode);
                        
                        _cycleBreathMode = _cycleBreathMode == CycleBreathMode.BreathIn ? CycleBreathMode.BreathOut : CycleBreathMode.BreathIn;
                        SunEventManager.EmitEvent(EventID.BreathChangeCycle, sender: _cycleBreathMode);
                    }
                    SunEventManager.EmitEvent(EventID.BreathUpdate, sender: _currentCycleBreathIntSecond);
                }

                if (_currentBreathIntSecond >= _totalSecondInBreath)
                {
                    _isStartBreathMode = false;
                    SunEventManager.EmitEvent(EventID.BreathStop);
                }
            }
        }

        #endregion

        #region Focus mode

        private void OnFocusTimerModeStart()
        {
            _totalSecondInFocus = (int) SunEventManager.GetSender(EventID.FocusStart);
            _currentFocusFloatSecond = _totalSecondInFocus;
            _currentFocusIntSecond = _totalSecondInFocus;
            _isStartFocusMode = true;

            CurrentMode = Mode.Focus;
        }
        
        private void OnFocusTimerModeStop()
        {
            _isStartFocusMode = false;
            CurrentMode = Mode.None;

            var secondInPhase = _totalSecondInFocus - _currentFocusIntSecond;
            TotalSecondFocusMode += secondInPhase;
            
            SunUIController.GetScreen<PopupScreen>().SetText("tập trung", TimeSpan.FromSeconds(secondInPhase), TimeSpan.FromSeconds(TotalSecondFocusMode));
            SunUIController.PushScreen<PopupScreen>(hideCurrentScreen: false);
        }

        #endregion

        #region Sleep mode
        
        private void OnSleepModeStart()
        {
            _endSleepTimeSpan = (TimeSpan)SunEventManager.GetSender(EventID.SleepStart);
            _isStartSleepMode = true;

            CurrentMode = Mode.Sleep;
        }

        private void OnSleepModeStop()
        {
            _isStartSleepMode = false;

            CurrentMode = Mode.None;

            SunUIController.PushScreen<PopupSleepScreen>(hideCurrentScreen: false);
        }

        #endregion

        #region Breath mode

        private void OnBreathModeStart()
        {
            _totalSecondInBreath = (int) SunEventManager.GetSender(EventID.BreathStart);
            
            _currentBreathIntSecond = 0;
            _currentCycleBreathFloatSecond = SecondCycleInBreathMode;
            _currentCycleBreathIntSecond = Mathf.RoundToInt(SecondCycleInBreathMode);
            _cycleBreathMode = CycleBreathMode.BreathIn;

            _isStartBreathMode = true;
            CurrentMode = Mode.Breath;
        }

        private void OnBreathModeStop()
        {
            _isStartBreathMode = false;
            CurrentMode = Mode.None;

            TotalSecondBreathMode += _currentBreathIntSecond;
            
            SunUIController.GetScreen<PopupScreen>().SetText("hít thở", TimeSpan.FromSeconds(_currentBreathIntSecond), TimeSpan.FromSeconds(TotalSecondBreathMode));
            SunUIController.PushScreen<PopupScreen>(hideCurrentScreen: false);
        }

        #endregion
        
    }
}
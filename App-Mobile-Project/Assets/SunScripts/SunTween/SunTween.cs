using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace DunnGSunn
{
    public enum Style
    {
        Once,
        Loop,
        LoopWithCount
    }

    public enum Direction
    {
        Forward,
        Reverse
    }

    public enum EventWhen
    {
        None,
        Forward,
        Reverse,
        Both
    }

    public enum DelayWhen
    {
        None,
        Forward,
        Reverse,
        Both
    }
    
    public abstract class SunTween : MonoBehaviour
    {
        #region Fields

        [Header("Group ID"), Tooltip("Sử dụng để lấy reference nhanh tween trong Tween Control")]
        public int tweenGroupID;
        
        [Header("Active")]
        public bool isActive = true;
        
        [Header("Auto play tween")]
        public bool isAutoPlay;
        [HideIf("isAutoPlay", false)]
        public Direction direction = Direction.Forward;

        [Header("Target")] 
        public RectTransform mainTarget;
        
        [Header("Enable/Disable")]
        public bool enableBeforeForward;
        public bool disableAfterReverse;

        [Header("Tween style")] 
        public bool sameStyleInReverse = true;
        public Ease easeForward = Ease.OutBack;
        public Style styleForward = Style.Once;
        [HideIf("sameStyleInReverse", true)]
        public Ease easeReverse = Ease.InBack;
        [HideIf("sameStyleInReverse", true)]
        public Style styleReverse = Style.Once;

        [Header("Loop style"), Tooltip("Sử dụng khi dùng tween có vòng lặp.\n-1 lặp vô tận, khác -1 lặp theo chu kì")] 
        public LoopType loopStyle = LoopType.Yoyo;
        public int loopCount = -1;

        [Header("Delay")] 
        public DelayWhen delayWhen = DelayWhen.None;
        [HideIfEnumValue("delayWhen", HideIf.Equal, (int)EventWhen.None)]
        public float delay;
        
        [Header("Duration")]
        public float duration = .5f;
        
        [Header("Event trigger")] 
        public EventWhen startEventWhen = EventWhen.None;
        public EventWhen finishedEventWhen = EventWhen.None;
        [HideIfEnumValue("startEventWhen", HideIf.Equal, (int)EventWhen.None)]
        public UnityEvent onStart;
        [HideIfEnumValue("finishedEventWhen", HideIf.Equal, (int)EventWhen.None)]
        public UnityEvent onFinished;
        
        public bool Animating { get; set; }
        public Tween MainTween { get; set; }
        
        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (mainTarget == null) mainTarget = GetComponent<RectTransform>();
            LoadInAwake();
        }

        private void OnEnable()
        {
            if (mainTarget == null) mainTarget = GetComponent<RectTransform>();
            
            if (isActive && isAutoPlay)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        PlayForward();
                        break;
                    case Direction.Reverse:
                        PlayReverse();
                        break;
                }
            }
        }

        private void Start()
        {
            if (mainTarget == null) mainTarget = GetComponent<RectTransform>();
        }

        #endregion

        #region Tween functions

        public virtual void LoadInAwake() { }
        
        public abstract void PlayForward();
        public abstract void PlayReverse();
        public abstract void Stop(bool complete = false);
        
        public abstract void SetCurrentValueToStart();
        public abstract void SetCurrentValueToEnd();
        
        public abstract void SetStartToCurrentValue();
        public abstract void SetEndToCurrentValue();

        public void AddListenerToStart(UnityAction listener) => onStart.AddListener(listener);
        public void AddListenerToEnd(UnityAction listener) => onFinished.AddListener(listener);

        #endregion
    }
}
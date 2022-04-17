using UnityEngine;

namespace DunnGSunn
{
    public abstract class SunUI : MonoBehaviour
    {
        #region Touch control

        protected float _lastTimeClick;
        
        public virtual bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > _lastTimeClick + 0.5f;
                if (result) _lastTimeClick = Time.unscaledTime;
                return result;
            }
        }

        #endregion

        #region Fields

        [Header("Debug")] 
        public bool isDebug;

        [Header("Tween")] 
        public SunTweenControl tweenShow;
        public SunTweenControl tweenHide;

        public bool IsShow { get; set; }

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            Application.targetFrameRate = 60;
            IsShow = false;
        }

        #endregion

        #region Override functions

        public virtual void Initialize() { }

        public virtual void Show()
        {
            if (IsShow) return;
            if (isDebug) Debug.Log("Show " + gameObject.name);
            _lastTimeClick = Time.unscaledTime;
            if (tweenShow == null) gameObject.SetActive(true);
            else tweenShow.PlayForward();
            IsShow = true;
        }

        public virtual void Hide()
        {
            if (!IsShow) return;
            if (isDebug) Debug.Log("Hide " + gameObject.name);
            _lastTimeClick = Time.unscaledTime;
            if (tweenHide == null) gameObject.SetActive(false);
            else tweenHide.PlayReverse();
            IsShow = false;
        }

        #endregion
    }
}
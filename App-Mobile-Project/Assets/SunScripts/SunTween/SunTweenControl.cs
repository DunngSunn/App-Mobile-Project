using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DunnGSunn
{
    public class SunTweenControl : MonoBehaviour
    {
        #region Fields

        [Space]
        public int tweenGroupID;

        [Space] 
        public bool isActive = true;
        
        [Space]
        public bool enableBeforeForward = true;
        public bool disableAfterReverse = true;

        [Space] 
        public DelayWhen delayWhen = DelayWhen.None;
        [HideIfEnumValue("delayWhen", HideIf.Equal, (int)EventWhen.None)]
        public float delay;

        [Space] 
        public GameObject mainTarget;
        
        [Space]
        public List<SunTween> listTween;

        public bool Animating { get; private set; }
        public float Duration { get; private set; }
        
        private Sequence _allTweenSequence;

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (listTween == null)
            {
                listTween = new List<SunTween>();
                Debug.Log("Không có tween nào trong danh sách.");
            }

            if (mainTarget == null) mainTarget = transform.gameObject;
        }

        #endregion

        #region Tween functions

        public void PlayForward()
        {
            if (!isActive) return;
            if (listTween == null || listTween.Count <= 0) return;
            if (enableBeforeForward) mainTarget.SetActive(true);
            
            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in listTween) 
            {
                tween.PlayForward();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }
            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                .OnStart(() => { Animating = true; })
                .OnComplete(() => { Animating = false; })
                .Play();
        }
        
        public void PlayReverse()
        {
            if (!isActive) return;
            if (listTween == null || listTween.Count <= 0) return;
            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in listTween) 
            {
                tween.PlayReverse();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }
            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                .OnStart(() => { Animating = true; })
                .OnComplete(() => { Animating = false; if (disableAfterReverse) mainTarget.SetActive(false); })
                .Play();
        }

        public void Stop(bool complete = false)
        {
            _allTweenSequence?.Kill(complete);
        }

        [ContextMenu("Add tween on children")]
        public void AddTweenOnChildren()
        {
            if (listTween == null) listTween = new List<SunTween>();
            listTween.Clear();
            
            var allTweenInChild = transform.GetComponentsInChildren<SunTween>();
            foreach (var tweenHelper in allTweenInChild)
            {
                if (tweenHelper.tweenGroupID == tweenGroupID)
                {
                    listTween.Add(tweenHelper);
                }
            }
        }
        
        #endregion
    }
}
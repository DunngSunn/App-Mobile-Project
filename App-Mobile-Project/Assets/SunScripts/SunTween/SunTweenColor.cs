using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DunnGSunn
{
    public class SunTweenColor : SunTween
    {
        #region Fields

        [Space] 
        public Color from = Color.black;
        public Color to = Color.white;

        private Graphic _graphic;

        #endregion

        #region Unity callback functions

        public override void LoadInAwake()
        {
            _graphic = mainTarget.GetComponent<Graphic>();
        }

        #endregion

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenForward(Color f, Color t)
        {
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete())
                {
                    MainTween.Kill();
                }
            }
            else
            {
                _graphic.color = f;
            }
            if (enableBeforeForward) mainTarget.gameObject.SetActive(true);
            switch (styleForward)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                        .SetEase(easeForward)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both) 
                                onFinished?.Invoke();
                            Animating = false;
                        });
                    break;
                case Style.Loop:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                        .SetLoops(-1, loopStyle)
                        .SetEase(easeForward)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        });
                    break;
                case Style.LoopWithCount:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                        .SetLoops(loopCount * 2, loopStyle)
                        .SetEase(easeForward)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both) 
                                onFinished?.Invoke();
                            Animating = false;
                        });
                    break;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenReverse(Color f, Color t)
        {
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete())
                {
                    MainTween.Kill();
                }
            }
            else
            {
                _graphic.color = f;
            }
            switch (sameStyleInReverse ? styleForward : styleReverse)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                        .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both) 
                                onFinished?.Invoke();
                            Animating = false;
                            if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
                        });
                    break;
                case Style.Loop:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                        .SetLoops(-1, loopStyle)
                        .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        });
                    break;
                case Style.LoopWithCount:
                    MainTween = _graphic.DOColor(t, duration)
                        .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                        .SetLoops(loopCount * 2, loopStyle)
                        .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                        .OnStart(() =>
                        {
                            if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both) 
                                onStart?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both) 
                                onFinished?.Invoke();
                            Animating = false;
                            if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
                        });
                    break;
            }
        }

        #region Implement tween functions

        /// Play forward
        public override void PlayForward()
        {
            if (!isActive) return;
            SetTweenForward(@from, to);
        }

        /// Play reverse
        public override void PlayReverse()
        {
            if (!isActive) return;
            SetTweenReverse(to, @from);
        }

        public override void Stop(bool complete = false)
        {
            MainTween.Kill(complete);
        }

        [ContextMenu("Set current value to 'From'")]
        public override void SetCurrentValueToStart()
        {
            var target = GetComponent<Graphic>();
            @from = target.color;
        }

        [ContextMenu("Set current value to 'To'")]
        public override void SetCurrentValueToEnd()
        {
            var target = GetComponent<Graphic>();
            to = target.color;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = @from;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = to;
        }

        #endregion
    }
}
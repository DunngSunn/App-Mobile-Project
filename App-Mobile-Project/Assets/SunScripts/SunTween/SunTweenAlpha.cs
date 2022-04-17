using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DunnGSunn
{
    public class SunTweenAlpha : SunTween
    {
        #region Fields

        [Space] 
        public float from;
        public float to;

        public bool isCanvasGroup;

        public Graphic graphic;
        public CanvasGroup canvasGroup;

        #endregion

        #region Unity callback functions

        public override void LoadInAwake()
        {
            if (isCanvasGroup)
            {
                canvasGroup = mainTarget.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = mainTarget.gameObject.AddComponent<CanvasGroup>();
            }
            else
            {
                graphic = mainTarget.GetComponent<Graphic>();
            }
        }

        #endregion

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenForward(float f, float t)
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
                if (isCanvasGroup) canvasGroup.alpha = f;
                else graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, @f);
            }
            if (enableBeforeForward) mainTarget.gameObject.SetActive(true);
            if (isCanvasGroup)
            {
                switch (styleForward)
                {
                    case Style.Once:
                        MainTween = canvasGroup.DOFade(t, duration)
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
                        MainTween = canvasGroup.DOFade(t, duration)
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
                        MainTween = canvasGroup.DOFade(t, duration)
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
            else
            {
                switch (styleForward)
                {
                    case Style.Once:
                        MainTween = graphic.DOFade(t, duration)
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
                        MainTween = graphic.DOFade(t, duration)
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
                        MainTween = graphic.DOFade(t, duration)
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
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenReverse(float f, float t)
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
                if (isCanvasGroup) canvasGroup.alpha = f;
                else graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, @f);
            }
            if (isCanvasGroup)
            {
                switch (sameStyleInReverse ? styleForward : styleReverse)
                {
                    case Style.Once:
                        MainTween = canvasGroup.DOFade(t, duration)
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
                        MainTween = canvasGroup.DOFade(t, duration)
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
                        MainTween = canvasGroup.DOFade(t, duration)
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
            else
            {
                switch (sameStyleInReverse ? styleForward : styleReverse)
                {
                    case Style.Once:
                        MainTween = graphic.DOFade(t, duration)
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
                        MainTween = graphic.DOFade(t, duration)
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
                        MainTween = graphic.DOFade(t, duration)
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
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                @from = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                @from = target.color.a;
            }
        }

        [ContextMenu("Set current value to 'To'")]
        public override void SetCurrentValueToEnd()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                to = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                to = target.color.a;
            }
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = @from;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, @from);
            }
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = to;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, to);
            }
        }

        #endregion
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace DunnGSunn
{
    public class SunToggle : MonoBehaviour
    {
        #region Fields

        private Toggle _toggle;

        [Header("Using tween or not")]
        public bool useTween;
        
        [Space]
        [HideIf(nameof(useTween), false)]
        public SunTween toggleTweenObjectTrue;
        [HideIf(nameof(useTween), false)]
        public SunTween toggleTweenObjectFalse;
        [HideIf(nameof(useTween), false)]
        public Graphic graphicsObjectTrue;
        [HideIf(nameof(useTween), false)]
        public Graphic graphicsObjectFalse;
        
        [Space]
        [HideIf(nameof(useTween), true)]
        public GameObject toggleObjectTrue;
        [HideIf(nameof(useTween), true)]
        public GameObject toggleObjectFalse;

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }
        
        private void Reset()
        {
            if (useTween)
            {
                toggleTweenObjectFalse = transform.Find("False").GetComponent<SunTween>();
                toggleTweenObjectTrue = transform.Find("True").GetComponent<SunTween>();

                toggleObjectFalse = null;
                toggleObjectTrue = null;
            }
            else
            {
                toggleObjectFalse = transform.Find("False").gameObject;
                toggleObjectTrue = transform.Find("True").gameObject;

                toggleTweenObjectFalse = null;
                toggleTweenObjectTrue = null;
            }
        }
        
        private void Start()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        #endregion

        #region Toggle control functions

        public void InitializeToggle()
        {
            if (useTween)
            {
                toggleTweenObjectFalse.gameObject.SetActive(true);
                toggleTweenObjectTrue.gameObject.SetActive(false);
                
                graphicsObjectTrue.color = Color.white;
                graphicsObjectFalse.color = Color.white;

                _toggle.isOn = false;
            }
            else
            {
                toggleObjectFalse.SetActive(true);
                toggleObjectTrue.SetActive(false);
                
                _toggle.isOn = false;
            }
        }

        private void OnToggleValueChanged(bool value)
        {
            if (useTween)
            {
                if (value)
                {
                    toggleTweenObjectTrue.PlayForward();
                    toggleTweenObjectFalse.PlayReverse();
                }
                else
                {
                    toggleTweenObjectTrue.PlayReverse();
                    toggleTweenObjectFalse.PlayForward();
                }
            }
            else
            {
                toggleObjectTrue.SetActive(value);
                toggleObjectFalse.SetActive(!value);
            }
        }

        #endregion
    }
}
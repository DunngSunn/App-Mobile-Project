using DG.Tweening;
using DunnGSunn;
using TMPro;
using UnityEngine;

namespace Theme
{
    public class ThemeController : MonoBehaviour
    {
        #region Fields

        [Header("Name theme")] 
        public string nameTheme;

        [Header("Text")] 
        public TextMeshProUGUI textNameTheme;

        private bool _isShowingName;
        private float _timeHideName;

        #endregion

        #region Unity callback functions

        private void Reset()
        {
            var nameSplit = transform.name.Split(' ');
            nameTheme = nameSplit[nameSplit.Length - 1];

            textNameTheme = transform.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        private void Update()
        {
            if (_isShowingName)
            {
                _timeHideName -= Time.deltaTime;
                if (_timeHideName <= 0)
                {
                    _isShowingName = false;
                    textNameTheme.DOFade(0f, .5f);
                }
            }
        }

        #endregion

        #region Theme control functions

        public void InitializeTheme()
        {
            textNameTheme.text = nameTheme;
            textNameTheme.color = new Color(1f, 1f, 1f, 0f);
            _isShowingName = false;
        }

        public void PlayTheme()
        {
            SunEventManager.EmitEvent(EventID.ThemeAudioPlay, sender: nameTheme);

            textNameTheme.DOFade(1f, .5f).OnComplete(() =>
            {
                _isShowingName = true;
                _timeHideName = 3f;
            });
        }

        public void StopTheme()
        {
            SunEventManager.EmitEvent(EventID.ThemeAudioStop, sender: nameTheme);

            if (_isShowingName)
            {
                _isShowingName = false;
                textNameTheme.DOFade(0f, .5f);
            }
        }

        #endregion
    }
}
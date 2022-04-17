using System.Collections.Generic;
using DunnGSunn;
using Theme;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Manager
{
    public class ThemeManager : SunMonoSingleton<ThemeManager>, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields

        [Header("Horizontal scroll")]
        public HorizontalScrollSnap scrollSnap;

        [Header("List theme controller")]
        public List<ThemeController> themeControllers;

        private ThemeController _currentTheme;
        private ThemeController _nextTheme;
        
        private ThemeController _currentPointerTheme;

        private Vector2 _currentPointer;
        private Vector2 _endPointer;

        private bool _isPlayingTheme;

        #endregion

        #region Unity callback functions

        private void Start()
        {
            foreach (var themeController in themeControllers)
            {
                themeController.InitializeTheme();
            }

            _currentTheme = themeControllers[scrollSnap.StartingScreen];

            _isPlayingTheme = false;
        }
        
        #endregion

        #region Event scroll view

        public void OnSelectionChangeStartEvent()
        {
            _nextTheme = null;
        }

        public void OnSelectionPageChangedEvent()
        {
            _nextTheme = themeControllers[scrollSnap.CurrentPage];
            
            if (_nextTheme != null && _currentTheme != _nextTheme)
            {
                _currentTheme.StopTheme();
                _nextTheme.PlayTheme();
                _currentTheme = _nextTheme;
                _nextTheme = null;
                _isPlayingTheme = true;
            }
        }

        #endregion

        #region Pointer event

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentPointerTheme = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<ThemeController>();
            _currentPointer = eventData.pointerCurrentRaycast.screenPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _endPointer = eventData.pointerCurrentRaycast.screenPosition;
            if (_currentPointerTheme != null && Vector2.Distance(_currentPointer, _endPointer) <= 1f)
            {
                _isPlayingTheme = !_isPlayingTheme;
                if (_isPlayingTheme)
                {
                    _currentPointerTheme.PlayTheme();
                }
                else
                {
                    _currentPointerTheme.StopTheme();
                }
            }
        }

        #endregion
    }
}
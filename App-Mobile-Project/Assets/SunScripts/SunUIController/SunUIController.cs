using System.Collections.Generic;
using UnityEngine;

namespace DunnGSunn
{
    [DisallowMultipleComponent]
    public class SunUIController : SunMonoSingleton<SunUIController>
    {
        #region Fields

        [Header("UI screens inGame")]
        public SunUI[] uiScreens;
        public SunUI startingScreen;
        
        private Stack<SunUI> _uiStack = new Stack<SunUI>();

        #endregion

        #region UI control

        /// <summary>
        /// Lấy UI trong list
        /// </summary>
        public static T GetScreen<T>() where T : SunUI
        {
            foreach (var gameUI in Instance.uiScreens)
            {
                if (gameUI is T tScreen)
                {
                    return tScreen;
                }
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra UI có trong stack không?
        /// </summary>
        public static bool IsScreenInStack<T>() where T : SunUI
        {
            return Instance._uiStack.Contains(GetScreen<T>());
        }

        /// <summary>
        /// Kiểm tra UI có ở đầu stack không?
        /// </summary>
        public static bool IsScreenOnTopOfStack<T>() where T : SunUI
        {
            return Instance._uiStack.Count > 0 && GetScreen<T>() == Instance._uiStack.Peek();
        }

        /// <summary>
        /// Hiển thị UI khởi đầu
        /// </summary>
        public static void PushStartingScreen(SunUI startingScreen)
        {
            startingScreen.Show();
            Instance._uiStack.Push(startingScreen);
        }

        /// <summary>
        /// Hiển thị UI mới
        /// </summary>
        public static void PushScreen<T>(bool hideCurrentScreen) where T : SunUI
        {
            var thisScreen = GetScreen<T>();
            thisScreen.Show();

            if (Instance._uiStack.Count > 0)
            {
                var currentScreen = Instance._uiStack.Peek();
                if (hideCurrentScreen) currentScreen.Hide();
            }

            Instance._uiStack.Push(thisScreen);
        }
        
        /// <summary>
        /// Hiển thị UI mới
        /// </summary>
        public static void PushScreen<T>(bool hideCurrentScreen, bool hideSecondScreen) where T : SunUI
        {
            var thisScreen = GetScreen<T>();
            thisScreen.Show();

            if (Instance._uiStack.Count > 0)
            {
                var currentScreen = Instance._uiStack.Pop();
                var secondScreen = Instance._uiStack.Peek();
                
                if (hideSecondScreen) secondScreen.Hide();
                if (hideCurrentScreen) currentScreen.Hide();

                Instance._uiStack.Push(currentScreen);
            }

            Instance._uiStack.Push(thisScreen);
        }

        /// <summary>
        /// Ẩn UI đang hiển thị và trở về UI trước đó
        /// </summary>
        public static void PopScreen()
        {
            if (Instance._uiStack.Count > 1)
            {
                var currentScreen = Instance._uiStack.Pop();
                currentScreen.Hide();

                var newCurrentScreen = Instance._uiStack.Peek();
                newCurrentScreen.Show();
            }
        }
        
        /// <summary>
        /// Ẩn UI đang hiển thị và trở về UI trước đó
        /// </summary>
        public static void PopScreen(bool showSecondScreen)
        {
            if (Instance._uiStack.Count > 1)
            {
                var currentScreen = Instance._uiStack.Pop();
                currentScreen.Hide();

                var newCurrentScreen = Instance._uiStack.Pop();
                newCurrentScreen.Show();

                var secondScreen = Instance._uiStack.Peek();
                if (showSecondScreen) secondScreen.Show();

                Instance._uiStack.Push(newCurrentScreen);
            }
        }

        /// <summary>
        /// Ẩn tất cả UI cho đến UI đầu
        /// </summary>
        public static void PopAllScreen()
        {
            for (var i = 0; i < Instance._uiStack.Count; i++)
            {
                PopScreen();
            }
        }

        #endregion

        #region Unity callback functions

        private void Start()
        {
            foreach (var sunUI in uiScreens)
            {
                sunUI.gameObject.SetActive(true);
                sunUI.Initialize();
                sunUI.gameObject.SetActive(false);
            }
            
            if (startingScreen != null)
            {
                PushStartingScreen(startingScreen);
            }
        }

        #endregion
    }
}
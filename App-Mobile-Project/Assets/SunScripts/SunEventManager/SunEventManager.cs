using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace DunnGSunn
{
    //ID của event cần sử dụng
    public static class EventID
    {
        // Theme
        public const string ThemeAudioPlay = "ThemeAudioPlay";
        public const string ThemeAudioStop = "ThemeAudioStop";
        
        // Focus mode
        public const string FocusStart = "FocusStart";
        public const string FocusUpdate = "FocusUpdate";
        public const string FocusStop = "FocusStop";
        
        // Sleep mode
        public const string SleepStart = "SleepStart";
        public const string SleepStop = "SleepStop";
        
        // Breath mode
        public const string BreathStart = "BreathStart";
        public const string BreathUpdate = "BreathUpdate";
        public const string BreathChangeCycle = "BreathChangeCycle";
        public const string BreathStop = "BreathStop";
        public const string BreathInAudio = "BreathInAudio";
        public const string BreathOutAudio = "BreathOutAudio";
    }

    public static class SunEventManager
    {
        #region Fields

        private static Dictionary<string, UnityEvent> _eventDictionary = new Dictionary<string, UnityEvent>();
        private static Dictionary<string, object> _storage = new Dictionary<string, object>();
        private static Dictionary<string, object> _sender = new Dictionary<string, object>();

        #endregion

        #region Start listening

        /// <summary>
        /// Đăng kí nghe sự kiện
        /// </summary>
        public static void StartListening(string eventName, UnityAction callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(callBack);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(callBack);
                _eventDictionary.Add(eventName, thisEvent);
            }
        }

        #endregion

        #region Stop listening

        /// <summary>
        /// Dừng nghe sự kiện
        /// </summary>
        public static void StopListening(string eventName, UnityAction callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(callBack);
            }
        }

        #endregion

        #region Emit event

        /// <summary>
        /// Phát sự kiện
        /// </summary>
        public static void EmitEvent(string eventName)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        /// <summary>
        /// Phát sự kiện có thời gian trễ
        /// </summary>
        public static void EmitEvent(string eventName, float delay)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                if (delay <= 0)
                {
                    thisEvent.Invoke();
                }
                else
                {
                    var d = (int) (delay * 1000);
                    DelayedInvoke(thisEvent, d);
                }
            }
        }

        /// <summary>
        /// Phát sự kiện với sender
        /// </summary>
        public static void EmitEvent(string eventName, object sender)
        {
            if (_sender.ContainsKey(eventName))
            {
                _sender[eventName] = sender;
            }
            else
            {
                _sender.Add(eventName, sender);
            }

            EmitEvent(eventName);
        }

        /// <summary>
        /// Phát sự kiện với sender có thời gian trễ
        /// </summary>
        public static void EmitEvent(string eventName, float delay, object sender)
        {
            if (_sender.ContainsKey(eventName))
            {
                _sender[eventName] = sender;
            }
            else
            {
                _sender.Add(eventName, sender);
            }

            EmitEvent(eventName, delay);
        }

        /// <summary>
        /// Phát sự kiện với data
        /// </summary>
        public static void EmitEventData(string eventName, object data)
        {
            SetData(eventName, data);
            EmitEvent(eventName);
        }

        /// <summary>
        /// Phát sự kiện với data có thời gian trễ
        /// </summary>
        public static void EmitEventData(string eventName, float delay, object data)
        {
            SetData(eventName, data);
            EmitEvent(eventName, delay);
        }

        #endregion

        #region Get/Set data

        /// <summary>
        /// Thiết lập data cho event
        /// </summary>
        public static void SetData(string eventName, object data)
        {
            if (_storage.ContainsKey(eventName))
            {
                _storage[eventName] = data;
            }
            else
            {
                _storage.Add(eventName, data);
            }
        }

        /// <summary>
        /// Lấy data của event kiểu object
        /// </summary>
        public static object GetData(string eventName)
        {
            try
            {
                return _storage.ContainsKey(eventName) ? _storage[eventName] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy data của event kiểu GameObject
        /// </summary>
        public static GameObject GetDataGameObject(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (GameObject) _storage[eventName];
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy data của event kiểu int
        /// </summary>
        public static int GetDataInt(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (int) _storage[eventName];
                }

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy data của event kiểu float
        /// </summary>
        public static float GetDataFloat(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (float) _storage[eventName];
                }

                return 0f;
            }
            catch (Exception)
            {
                return 0f;
            }
        }

        /// <summary>
        /// Lấy data của event kiểu bool
        /// </summary>
        public static bool GetDataBool(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (bool) _storage[eventName];
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy data của event kiểu string
        /// </summary>
        public static string GetDataString(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (string) _storage[eventName];
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy sender của event
        /// </summary>
        public static object GetSender(string eventName)
        {
            try
            {
                return _sender.ContainsKey(eventName) ? _sender[eventName] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delay thời gian invoke event
        /// </summary>
        private static async void DelayedInvoke(UnityEvent thisEvent, int delay)
        {
            await Task.Delay(delay);
            thisEvent.Invoke();
        }

        /// <summary>
        /// Xóa toàn bộ listeners có trong các event
        /// </summary>
        public static void StopAllListeners()
        {
            foreach (var unityEvent in _eventDictionary)
            {
                unityEvent.Value.RemoveAllListeners();
            }

            _eventDictionary = new Dictionary<string, UnityEvent>();
        }

        /// <summary>
        /// Kiểm tra còn sự kiện nào còn nghe nữa không?
        /// </summary>
        public static bool IsListening()
        {
            return _eventDictionary.Count > 0;
        }

        /// <summary>
        /// Kiểm tra có sự kiện không?
        /// </summary>
        public static bool EventExists(string eventName)
        {
            return _eventDictionary.ContainsKey(eventName);
        }

        #endregion
    }
}
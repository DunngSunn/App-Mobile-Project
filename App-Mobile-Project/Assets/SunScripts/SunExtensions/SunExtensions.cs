using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunnGSunn
{
    public static class SunExtensions
    {
        /// <summary>
        /// Thêm hoặc lấy component trong GameObject
        /// </summary>
        public static TComponent GetOrAddComponent<TComponent>(this GameObject target) where TComponent : Component
        {
            if (target == null)
                throw new ArgumentNullException("Target gameobject must not be null.");

            var comp = target.GetComponent<TComponent>();
            if (comp == null)
                comp = target.AddComponent<TComponent>();

            return comp;
        }
        
        /// <summary>
        /// Xáo trộn các phần tử có trong list
        /// </summary>
        public static void ShuffleList<T>(this System.Collections.Generic.List<T> list)
        {
            var rng = new System.Random();
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandom<T>(this List<T> list)
        {
            var randomElement = list[UnityEngine.Random.Range(0, list.Count)];
            return randomElement;
        }
        
        /// <summary>
        /// Đổi string sang int
        /// </summary>
        public static int TryParseInt(this string s, int defaultValue = 0)
        {
            var result = defaultValue;
            int.TryParse (s, out result);
            return result;
        }
        
        /// <summary>
        /// Đổi string sang float
        /// </summary>
        public static float TryParseFloat(this string s, float defaultValue = 0f)
        {
            var result = defaultValue;
            float.TryParse (s, out result);
            return result;
        }

        /// <summary>
        /// Lấy giá trị ngẫu nhiên trong enum
        /// </summary>
        public static T GetRandomEnumValue<T>()
        {
            var value = Enum.GetValues(typeof(T));
            var randomIndex = UnityEngine.Random.Range(0, value.Length);
            return (T) value.GetValue(randomIndex);
        }
    }
}
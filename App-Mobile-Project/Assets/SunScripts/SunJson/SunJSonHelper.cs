namespace DunnGSunn
{
    public static class SunJSonHelper
    {
        #region Json control functions

        public static T[] FromJson<T>(string json)
        {
            var wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {items = array};
            return UnityEngine.JsonUtility.ToJson(wrapper);
        }

        #endregion

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}

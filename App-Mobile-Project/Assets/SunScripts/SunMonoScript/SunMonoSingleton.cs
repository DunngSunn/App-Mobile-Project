using UnityEngine;

namespace DunnGSunn
{
    public abstract class SunMonoSingleton<T> : MonoBehaviour where T : SunMonoSingleton<T>
    {
        #region Fields

        [Header("Destroy on load")]
        [SerializeField] private bool dontDestroyOnLoad;

        public static T Instance { get; private set; }

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                LoadInAwake();
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Load trong hàm awake khi đã khởi tạo được instance
        /// </summary>
        protected virtual void LoadInAwake() { }

        #endregion
    }
}

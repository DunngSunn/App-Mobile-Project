using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DunnGSunn;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadSceneAsync : SunMonoSingleton<LoadSceneAsync>
    {
        #region Fields

        [Header("Tween")] 
        public SunTween tweenHideLoadingCanvas;

        [Header("Scenes")] 
        public string nameMainScene = "Main Scene";

        [Header("Back image")] 
        public Image backImage;
        public List<Sprite> backSprite;

        [Header("Quote")] 
        public TextMeshProUGUI quoteText;
        
        #endregion

        #region Unity callback functions

        private void Start()
        {
            tweenHideLoadingCanvas.finishedEventWhen = EventWhen.Reverse;
            tweenHideLoadingCanvas.AddListenerToEnd(() => Destroy(gameObject));

            var randomSprite = backSprite.GetRandom();
            backImage.sprite = randomSprite;

            var splitQuote = QuoteManager.Instance.GetQuote().Split('*');
            quoteText.text = $"{splitQuote[0]} \n {splitQuote[1]}";
            
            LoadScene(nameMainScene);
        }

        #endregion

        #region Load scene functions
        
        public async void LoadScene(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            do
            {
                await Task.Delay(150);
            } while (asyncLoad.progress < .9f);

            await Task.Delay(2000);
            
            asyncLoad.allowSceneActivation = true;
            
            await Task.Delay(1000);
            tweenHideLoadingCanvas.PlayReverse();
        }

        #endregion
    }
}
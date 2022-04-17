using System;
using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;

namespace Manager
{
    public class QuoteManager : SunMonoSingleton<QuoteManager>
    {
        #region Fields

        public int CurrentQuoteID
        {
            get => PlayerPrefs.GetInt("CurrentQuoteID", defaultValue: 0);
            set => PlayerPrefs.SetInt("CurrentQuoteID", value);
        }
        
        [Header("List quote")]
        public List<QuoteClass> quoteList;

        private QuoteClass _tempQuote;

        #endregion
        
        protected override void LoadInAwake()
        {
            // Check xem có câu quote nào trong danh sách không
            if (quoteList.Count > 0)
            {
                // Lấy ngẫu nhiên câu quote cho lần mở app này
                do
                {
                    _tempQuote = quoteList.GetRandom();
                } while (_tempQuote.id == CurrentQuoteID);
            }
            else
            {
                _tempQuote = new QuoteClass()
                {
                    id = 0,
                    quote = "Chưa có câu nào đâu."
                };
            }
                
            CurrentQuoteID = _tempQuote.id;
        }

        // Lấy quote
        public string GetQuote() => _tempQuote.quote;
    }

    [Serializable]
    public class QuoteClass
    {
        public int id;
        public string quote;
    }
}
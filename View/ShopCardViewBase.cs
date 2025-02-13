namespace Game.UI.IAPShop
{
    using Zenject;
    using TMPro;
    using UnityEngine.UI;
    using System;
    using UniRx;
    using UnityEngine;

    public interface IShopCardView
    {
        IObservable<Unit> OnShopCardClicked { get; }
        void SetPrice(string price);
    }
    
    [RequireComponent(typeof(Button))]
    public abstract class ShopCardViewBase : MonoBehaviour, IShopCardView
    {
        #region Extrenal

        [Inject] protected readonly Button _button;

        #endregion
        
        [SerializeField] private TextMeshProUGUI _priceText;
        
        public IObservable<Unit> OnShopCardClicked => _button.OnClickAsObservable();
        
        public void SetPrice(string price)
        {
            _priceText.text = price;
        }

        protected void SetText(TextMeshProUGUI text, string count)
        {
            text.text = count;
        }
    }
}
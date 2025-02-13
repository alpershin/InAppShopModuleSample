namespace Game.UI.IAPShop
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    
    public interface IShopCardResourceView : IShopCardView
    {
        void SetName(string bundleName);
        void SetCount(string count);
        void SetIcon(Sprite icon);
    }

    public class ShopCardResourceView : ShopCardViewBase, IShopCardResourceView
    {
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _icoImage;

        public void SetName(string bundleName) => SetText(_name, bundleName);

        public void SetCount(string count) => SetText(_countText, count);

        public void SetIcon(Sprite icon) => _icoImage.sprite = icon;
    }
}
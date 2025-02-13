namespace Game.Configs
{
    using System.Globalization;
    using Sirenix.Utilities;
    using IAP;
    using UnityEngine;
    using System.Collections.Generic;
    using Gley.EasyIAP;
    using Sirenix.OdinInspector;

    [CreateAssetMenu( fileName = "ShopConfig", menuName = "Configs/Shop")]
    public class ShopConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<ShopProductNames, ShopModelBase> _offers;

        private void OnValidate()
        {
            _offers.ForEach(pair =>
            {
                if (pair.Value is null) return;
                pair.Value.Type = pair.Key;
            });
        }

        public ShopModelBase GetConfig(ShopProductNames type)
        {
            return _offers.GetValueOrDefault(type);
        }
        
        public string GetCost(ShopProductNames productName)
        {
            return !_offers[productName].IsCanBuyByHard 
                ? API.GetLocalizedPriceString(productName)
                : _offers[productName].Cost.ToString(CultureInfo.InvariantCulture);
        }
    }
}
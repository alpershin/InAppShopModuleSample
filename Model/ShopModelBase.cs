namespace Game.Configs.IAP
{
    using Game.Core.GPS;
    using UnityEngine;
    using Sirenix.OdinInspector;
    using Gley.EasyIAP;

    public abstract class ShopModelBase
    {
        public ShopProductNames Type;
        
        [SerializeField] [EnumToggleButtons] public EShopModelBasePurchaseType PaymentVariation { get; set; }
        
        [ShowIf("PaymentVariation", EShopModelBasePurchaseType.HardCurrency)]
        public double Cost;
        
        public float MeatMinLimit;
        public float GearMinLimit;
        public float MeatMultiplier;
        public float GearMultiplier;
        
        public bool IsCanBuyByHard => PaymentVariation is EShopModelBasePurchaseType.HardCurrency;
        
        public abstract void Visit(IPurchaseVisitor purchaseVisitor);

        public virtual float CalculateMeat(IGPSService gpsService)
        {
           return Mathf.Clamp(gpsService.GetGPSValueFor(CcyType.Soft) * MeatMultiplier, MeatMinLimit, float.MaxValue);
        }

        public virtual float CalculateGear(IGPSService gpsService)
        {
            return Mathf.Clamp(gpsService.GetGPSValueFor(CcyType.Gears) * GearMultiplier, GearMinLimit, float.MaxValue);
        }
    }
}
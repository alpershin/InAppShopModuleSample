namespace Game.Configs.IAP
{
    using Game.Core.GPS;
    using UnityEngine;

    public class ShopResourceModel : ShopModelBase
    {
        public string Name;
        public Sprite Icon;

        public override float CalculateMeat(IGPSService gpsService)
        {
            return Mathf.Floor((gpsService.GetGPSValueFor(CcyType.Soft) + MeatMinLimit) * MeatMultiplier);
        }
        
        public override float CalculateGear(IGPSService gpsService)
        {
            return Mathf.Floor((gpsService.GetGPSValueFor(CcyType.Gears) + GearMinLimit) * GearMultiplier);
        }
        
        public override void Visit(IPurchaseVisitor purchaseVisitor)
        {
            purchaseVisitor.Visit(this);
        }
    }
}
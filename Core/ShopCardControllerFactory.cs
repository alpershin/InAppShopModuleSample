namespace Game.Configs.IAP
{
    using System;
    
    public interface IShopCardControllerFactory
    {
        ShopCardControllerBase GetController(ShopModelBase modelBase);
    }
    
    public class ShopCardControllerFactory : IShopCardControllerFactory
    {
        public ShopCardControllerBase GetController(ShopModelBase modelBase)
        {
            return modelBase switch
            {
                ShopAmplificationModel shopAmplificationModel => new ShopCardAmplificationController(),
                ShopCrystalModel shopCrystalModel => new ShopCardCrystalController(),
                ShopKeysModel shopKeysModel => new ShopCardKeysController(),
                ShopResourceModel shopResourceModel => new ShopCardResourceController(),
                ShopSetsModel shopSetsModel => new ShopCardSetsController(),
                ShopSpecialOfferModel shopSpecialOfferModel => new ShopCardSpecialOfferController(),
                _ => throw new ArgumentOutOfRangeException($"{modelBase.GetType()}")
            };
        }
    }
}
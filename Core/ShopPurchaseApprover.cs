namespace Game.Configs.IAP
{
    using Gley.EasyIAP.Internal;
    using Game.Core;
    using Game.Core.GPS;
    using Game.IAP;
    using Game.Profiles;
    
    public interface IPurchaseVisitor
    {
        void Visit(ShopSpecialOfferModel model);
        void Visit(ShopAmplificationModel model);
        void Visit(ShopCrystalModel model);
        void Visit(ShopResourceModel model);
        void Visit(ShopSetsModel model);
        void Visit(ShopKeysModel model);
    }
    
    public class ShopPurchaseApprover : IPurchaseVisitor
    {
        #region External
        
        private readonly GameProfile _gameProfile;
        private readonly IGameProfileManager _gameProfileManager;
        private readonly IGPSService _gpsService;
        private readonly IIAPService _iapService;
        private readonly IShopAmplificationConfig _amplificationConfig;
        private readonly EasyIAPData _easyIAP;
        
        #endregion

        public ShopPurchaseApprover(GameProfile gameProfile,
            IGameProfileManager gameProfileManager,
            IGPSService gpsService,
            IIAPService iapService,
            IShopAmplificationConfig amplificationConfig,
            EasyIAPData easyIAP)
        {
            _gameProfile = gameProfile;
            _gameProfileManager = gameProfileManager;
            _gpsService = gpsService;
            _iapService = iapService;
            _amplificationConfig = amplificationConfig;
            _easyIAP = easyIAP;
        }
        
        public void Visit(ShopSpecialOfferModel model)
        {
            _iapService.Buy(model.Type, () =>
            {
                _gameProfile.HardCurrency.Value += model.GemCount;
                _gameProfile.LightBlueFlash.Value += model.BlueFlashCount;
                _gameProfile.PurpleFlash.Value += model.PurpleFlashCount;

                _gameProfile.SoftCurrency.Value += (long)model.CalculateMeat(_gpsService);
                _gameProfile.GearsCurrency.Value += (int)model.CalculateGear(_gpsService);

                _gameProfile.Offer.StarterOfferProduct.Value = true;
                _gameProfileManager.Save();
            });
        }

        public void Visit(ShopAmplificationModel model)
        {
            _amplificationConfig.TryBuyUpgrade(model.EUpgradeType);
        }

        public void Visit(ShopCrystalModel model)
        {
            _iapService.Buy(model.Type, () =>
            {
                _gameProfile.HardCurrency.Value += (int)model.GemCount;
            }); 
        }

        public void Visit(ShopResourceModel model)
        {
            if (_gameProfile.HardCurrency.Value < model.Cost) return;
            
            if (model.Type.ToString().Contains("Meat"))
                _gameProfile.SoftCurrency.Value += (int)model.CalculateMeat(_gpsService);
            else if (model.Type.ToString().Contains("Gear"))
                _gameProfile.GearsCurrency.Value += (int)model.CalculateGear(_gpsService);
            else
                return;
            
            _gameProfile.HardCurrency.Value -= (int)model.Cost;
            _gameProfileManager.Save();
        }

        public void Visit(ShopSetsModel model)
        {
            _iapService.Buy(model.Type, () =>
            {
                _gameProfile.HardCurrency.Value += (int)model.GemCount;
                _gameProfile.GearsCurrency.Value += (int)model.CalculateGear(_gpsService);
                _gameProfile.SoftCurrency.Value += (long)model.CalculateMeat(_gpsService);
                
                _gameProfileManager.Save();
            });
        }

        public void Visit(ShopKeysModel model)
        {
            _iapService.Buy(model.Type, () =>
            {
                var name = model.Type.ToString();
                
                if (name.Contains("Meat"))
                {
                    if (model.Count <= 0)
                        _gameProfile.Offer.MeatADSkipInfinity.Value = true;
                    
                    _gameProfile.MeatADSkip.Value += model.Count;
                }
                else if (name.Contains("Gear"))
                {
                    if (model.Count <= 0)
                        _gameProfile.Offer.GearADSkipInfinity.Value = true;
                    
                    _gameProfile.GearADSkip.Value += model.Count;
                }
                else if (name.Contains("Flash"))
                {
                    if (model.Count <= 0)
                        _gameProfile.Offer.FlashADSkipInfinity.Value = true;
                    
                    _gameProfile.FlashADSkip.Value += model.Count;
                }
                else
                {
                    return;
                }
                
                _gameProfileManager.Save();
            });
        }
    }
}
namespace Game.Configs.IAP
{
    using Gley.EasyIAP.Internal;
    using Game.Core;
    using Game.IAP;
    using Game.Profiles;
    using Game.Core.GPS;
    using System;
    using UniRx;
    using Game.UI.IAPShop;
    using Gley.EasyIAP;
    using Zenject;

    public interface IShopCardController
    {
        
    }
    
    public abstract class ShopCardControllerBase : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _lifetimeDisposable = new CompositeDisposable();
        
        #region External

        [Inject] protected readonly IGPSService _gpsService;
        [Inject] protected readonly ShopConfig _shopConfig;
        [Inject] protected readonly ShopProductNames _productType;
        [Inject] protected readonly IShopCardView _viewBase;
        [Inject] protected readonly ShopModelBase _modelBase;
        [Inject] protected GameProfile _gameProfile;
        [Inject] private IGameProfileManager _gameProfileManager;
        [Inject] private readonly IIAPService _iapService;
        [Inject] private readonly IShopAmplificationConfig _amplificationConfig;
        [Inject] private readonly EasyIAPData _easyIAP;
        
        #endregion

        private IPurchaseVisitor _purchaseApprover;
        
        public virtual void Initialize()
        {
            _purchaseApprover = new ShopPurchaseApprover(_gameProfile, _gameProfileManager, _gpsService, _iapService, _amplificationConfig, _easyIAP);
            
            _viewBase.OnShopCardClicked
                .Subscribe(_ =>
                {
                    _modelBase.Visit(_purchaseApprover);
                })
                .AddTo(_lifetimeDisposable);
            
            _gameProfile.Upgrades.Values
                .CombineLatest()
                .Subscribe(_ => UpdateView())
                .AddTo(_lifetimeDisposable);
            
            UpdateView();
            SetPrice();
        }

        public void Dispose()
        {
            _lifetimeDisposable?.Dispose();
        }

        protected virtual void SetPrice()
        {
            _viewBase.SetPrice(_shopConfig.GetCost(_productType));
        }
        
        protected abstract void UpdateView();
    }
}
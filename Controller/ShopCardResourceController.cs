namespace Game.Configs.IAP
{
    using Game.Utilities;
    using Gley.EasyIAP;
    using Game.UI.IAPShop;

    public class ShopCardResourceController : ShopCardControllerBase
    {
        private IShopCardResourceView _view => (IShopCardResourceView)_viewBase;
        private ShopResourceModel _model => (ShopResourceModel)_modelBase;

        public override void Initialize()
        {
            base.Initialize();
            
            _view.SetName(_model.Name);
            _view.SetIcon(_model.Icon);
        }

        protected override void UpdateView()
        {
            _view.SetCount(
                _productType is ShopProductNames.SmallMeatSet or ShopProductNames.MediumMeatSet
                    or ShopProductNames.BigMeatSet
                    ? MoneyConversion.FloatConversion(_model.CalculateMeat(_gpsService))
                    : MoneyConversion.FloatConversion(_model.CalculateGear(_gpsService)));
        }
    }
}
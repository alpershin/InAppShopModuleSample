namespace Game.Installers
{
    using Game.Configs.IAP;
    using Game.Configs;
    using UnityEngine.UI;
    using UnityEngine;
    using Zenject;
    using Gley.EasyIAP;
    using Game.UI.IAPShop;
    
    [RequireComponent(typeof(Button))]
    public class ShopCardInstaller : MonoInstaller
    {
        #region External

        [Inject] private readonly ShopConfig _shopConfig;

        #endregion
        
        [SerializeField] private ShopProductNames _productName;

        public override void InstallBindings()
        {
            IShopCardControllerFactory controllerFactory = new ShopCardControllerFactory();
            var model = _shopConfig.GetConfig(_productName);
            
            Container
                .BindInstance(_productName)
                .AsSingle();
            Container
                .BindInstance(GetComponent<Button>())
                .AsSingle();

            Container
                .BindInstance(model)
                .AsSingle();
            
            Container
                .BindInterfacesTo<ShopCardViewBase>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .BindInterfacesTo(controllerFactory.GetController(model).GetType())
                .AsSingle();
        }
    }
}
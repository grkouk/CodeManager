﻿using GrKouk.CodeManager.Services;
using Prism;
using Prism.Ioc;
using GrKouk.CodeManager.ViewModels;
using GrKouk.CodeManager.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GrKouk.CodeManager
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk0MDUyQDMxMzcyZTM0MmUzMEQwczdzaGFzZ3pqN25QamprWVRQc3Zyb0xLMDlVMEdGYVJnYkQzc3NxZmM9");
            InitializeComponent();

            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
            await NavigationService.NavigateAsync(nameof(HomePage) + "/" + nameof(NavigationPage) + "/" + nameof(Views.MainPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDataSource, CodeManagerDataSource>();
            //containerRegistry.Register<ICodeDataSource,CodeDataSource>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductCodePage, ProductCodePageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductAttrCopy, ProductAttrCopyViewModel>();
            containerRegistry.RegisterForNavigation<UrlBuilderPage, UrlBuilderPageViewModel>();
            containerRegistry.RegisterForNavigation<NopProductDetailsPage, NopProductDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<NopProductImages, NopProductImagesViewModel>();
            containerRegistry.RegisterForNavigation<ShopToolsPage, ShopToolsPageViewModel>();
            containerRegistry.RegisterForNavigation<NopFeaturedProductsPage, NopFeaturedProductsPageViewModel>();
        }
    }
}

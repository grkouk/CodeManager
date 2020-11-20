using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GrKouk.CodeManager.Helpers;
using GrKouk.CodeManager.Views;
using Xamarin.Forms;

namespace GrKouk.CodeManager.ViewModels
{
   
    public class HomePageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public ObservableCollection<MyMenuItem> MenuItems { get; set; }
        private MyMenuItem _selectedMenuItem;
        public MyMenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }

        public DelegateCommand NavigateCommand { get; private set; }
        public HomePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            MenuItems = new ObservableCollection<MyMenuItem>();

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewa",
                PageName = nameof(MainPage),
                Title = "Home"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(ProductsPage),
                Title = "Products Page"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(ProductCodePage),
                Title = "Lookup Code"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(UrlBuilderPage),
                Title = "Build Url"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(NopProductDetailsPage),
                Title = "Nop Product"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(NopProductImages),
                Title = "Nop Product Images"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(ShopToolsPage),
                Title = "Shop Tools"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(NopFeaturedProductsPage),
                Title = "Nop Featured Products"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ic_viewb",
                PageName = nameof(SettingsPage),
                Title = "Settings Page"
            });
            NavigateCommand = new DelegateCommand(Navigate);
        }
        async void Navigate()
        {
            //nameof(HomePage) + "/" +
            //await _navigationService.NavigateAsync(nameof(HomePage) + "/" + nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
            try
            {
                await _navigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // await _navigationService.NavigateAsync( SelectedMenuItem.PageName);

        }
    }
}

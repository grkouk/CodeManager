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
                PageName = nameof(ProductCodePage),
                Title = "Product Page"
            });
            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = "ic_viewb",
            //    PageName = nameof(ProductPage),
            //    Title = "Products Test"
            //});
            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = "ic_viewb",
            //    PageName = nameof(ProductListPage),
            //    Title = "Product List"
            //});
            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = "ic_viewb",
            //    PageName = nameof(SettingsPage),
            //    Title = "Settings"
            //});

            NavigateCommand = new DelegateCommand(Navigate);
        }
        async void Navigate()
        {
            await _navigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
            // await _navigationService.NavigateAsync( SelectedMenuItem.PageName);

        }
    }
}

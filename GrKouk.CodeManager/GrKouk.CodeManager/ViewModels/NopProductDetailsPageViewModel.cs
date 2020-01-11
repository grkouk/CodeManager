using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Helpers;
using GrKouk.CodeManager.Models;
using GrKouk.CodeManager.Services;
using GrKouk.Shared.Definitions;
using Prism.Services;
using Syncfusion.Licensing;

namespace GrKouk.CodeManager.ViewModels
{
    public class NopProductDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;

        public NopProductDetailsPageViewModel(INavigationService navigationService, IPageDialogService dialogService
            , IDataSource dataSource) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataSource = dataSource;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (NopItems == null)
            {
                await RefreshDataAsync();
            }

            if (ShopList == null)
            {
                LoadShops();
                SelectedShopIndex = 1;
            }
        }


        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Shops

        private ObservableCollection<ListItemDto> _shopList;

        public ObservableCollection<ListItemDto> ShopList
        {
            get => _shopList;
            set => SetProperty(ref _shopList, value);
        }
        private int _selectedShopIndex;
        public int SelectedShopIndex
        {
            get => _selectedShopIndex;
            set => SetProperty(ref _selectedShopIndex, value);
        }
        private ListItemDto _selectedShop;
        public ListItemDto SelectedShop { get => _selectedShop; set => SetProperty(ref _selectedShop, value); }

        #region ShopValueChanged

        private DelegateCommand<object> _selectedShopIndexChangedCommand;
        public DelegateCommand<object> SelectedShopIndexChangedCommand =>
            _selectedShopIndexChangedCommand ?? (_selectedShopIndexChangedCommand = new DelegateCommand<object>((t) => SelectedShopIndexChangedCmd(t)));

        private void SelectedShopIndexChangedCmd(object value)
        {
            if (value != null)
            {

                if (value is string)
                {
                }
               
#if DEBUG
                    Debug.WriteLine("SelectedShopIndexChangedCmd");

                    try
                    {
                        var debugMessage = $"Value of value is {value.ToString()}";
                        Debug.WriteLine(debugMessage);
                    }
                    catch
                    {

                    }
#endif
                    //var productId = (value as ListItemDto).ItemCode;
               
               
            }
            else
            {
#if DEBUG
                Debug.WriteLine("SelectedShopIndexChangedCmd");

                try
                {
                    //var debugMessage = $"Selected index is value is {}";
                    //Debug.WriteLine(debugMessage);
                }
                catch
                {

                }
#endif
            }
        }

        #endregion

        private void LoadShops()
        {
            var items = Enum.GetValues(typeof(ShopEnum))
                .Cast<ShopEnum>()
                .Select(c => new ListItemDto()
                {
                    ItemCode = ((int)c).ToString(),
                    ItemName = c.GetEnumDescription()
                }).ToList();
            var cItems = new ObservableCollection<ListItemDto>();
            foreach (var item in items)
            {
                cItems.Add(item);
            }

            ShopList = cItems;

        }
        #endregion

        #region products
        private ObservableCollection<ProductListDto> _nopItems;

        public ObservableCollection<ProductListDto> NopItems
        {
            get => _nopItems;
            set => SetProperty(ref _nopItems, value);
        }
        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () => await RefreshDataCommand(), () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        async Task RefreshDataCommand()
        {
            await RefreshDataAsync();
        }
        private async Task RefreshDataAsync()
        {
            IsBusy = true;
            try
            {
                var nnItems = new ObservableCollection<ProductListDto>();
                var npItems = await GetNopItemsAsync();
                if (npItems != null)
                {
                    foreach (var item in npItems)
                    {
                        if (!String.IsNullOrEmpty(item.Code))
                        {
                            //_nopItems.Add(item);
                            nnItems.Add(item);
                        }
                    }
                }
                NopItems = nnItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task<IEnumerable<ProductListDto>> GetNopItemsAsync()
        {
            return await _dataSource.GetNopShopProductsAutocompleteListAsync("1");
        }

        private ProductListDto _selectedProductItem;
        public ProductListDto SelectedProductItem
        {
            get => _selectedProductItem;
            set => SetProperty(ref _selectedProductItem, value);
        }

        private DelegateCommand<object> _productValueChangedCommand;


        public DelegateCommand<object> ProductValueChangedCommand =>
            _productValueChangedCommand ?? (_productValueChangedCommand = new DelegateCommand<object>((t) => ProductValueChangedCmd(t)));

        private void ProductValueChangedCmd(object value)
        {

            if (value.GetType() == typeof(string))
            {

            }
            else if (value != null)
            {
                var productId = (value as ProductListDto).Id;


            }
            else
            {
                //CategoryText = string.Empty;
                // CostCentreText = string.Empty;
            }
        }
        #endregion
    }
}

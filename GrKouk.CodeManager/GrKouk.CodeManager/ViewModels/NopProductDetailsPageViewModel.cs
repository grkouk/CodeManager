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

using GrKouk.CodeManager.Services;
using GrKouk.Shared.Core;
using GrKouk.Shared.Definitions;
using GrKouk.Shared.Mobile.Dtos;
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

        private string _selectedShopId;

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
#if DEBUG
            Debug.WriteLine("SelectedShopIndexChangedCmd");
#endif
            if (value != null)
            {

                if (value is string)
                {
                }

                if (value is ListItemDto)
                {
                    _selectedShopId = (value as ListItemDto).ItemCode;
                    if (!IsBusy)
                    {
                        RefreshCommand.Execute();
                    }
                    
#if DEBUG
                    try
                    {
                        var debugMessage = $"Selected index is value is {_selectedShopId}";
                        Debug.WriteLine(debugMessage);
                    }
                    catch
                    {

                    }
#endif
                }

            }
            else
            {

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

        private int _selectedProductId;
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
            if (!String.IsNullOrEmpty( _selectedShopId))
            {
                return await _dataSource.GetNopShopProductsAutocompleteListAsync(_selectedShopId);
            }

            return null;
        }

        private int _selectedProductIndex;
        public int SelectedProductIndex
        {
            get => _selectedProductIndex;
            set => SetProperty(ref _selectedProductIndex, value);
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
#if DEBUG
            Debug.WriteLine("ProductValueChangedCmd");
#endif
            if (value != null)
            {

                if (value is ProductListDto)
                {
                    _selectedProductId = (value as ProductListDto).Id;
                    if (!IsBusy)
                    {
                        RefreshPicturesCommand.Execute();
                    }
#if DEBUG
                    try
                    {
                        var debugMessage = $"Selected index is value is {_selectedProductId}";
                        Debug.WriteLine(debugMessage);
                    }
                    catch
                    {

                    }
#endif
                }

            }
           

        }
        #endregion
        #region ProductPictures
        private ObservableCollection<ProductListDto> _productPictures;

        public ObservableCollection<ProductListDto> ProductPictures
        {
            get => _productPictures;
            set => SetProperty(ref _productPictures, value);
        }
        private DelegateCommand _refreshProductPicturesCommand;
        public DelegateCommand RefreshPicturesCommand =>
            _refreshProductPicturesCommand ?? (_refreshProductPicturesCommand = new DelegateCommand(async () => await RefreshProductPicturesCmd(), () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        async Task RefreshProductPicturesCmd()
        {
            await RefreshProductPicturesAsync();
        }
        private async Task RefreshProductPicturesAsync()
        {
            IsBusy = true;
            try
            {
                var nnItems = new ObservableCollection<ProductListDto>();
                var npItems = await GetNopProductPicturesAsync();
                if (npItems != null)
                {
                    foreach (var item in npItems)
                    {
                        if (!String.IsNullOrEmpty(item.ImageUrl))
                        {
                            //_nopItems.Add(item);
                            nnItems.Add(item);
                        }
                    }
                }
                ProductPictures = nnItems;
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
        private async Task<IEnumerable<ProductListDto>> GetNopProductPicturesAsync()
        {
            if (!String.IsNullOrEmpty(_selectedShopId) && _selectedProductId > 0)
            {
                return await _dataSource.GetNopShopProductPictureListAsync(_selectedShopId,_selectedProductId);
            }

            var emptyList = new List<ProductListDto>();
            return emptyList;
        }

        //private ProductListDto _selectedProductItem;
        //public ProductListDto SelectedProductItem
        //{
        //    get => _selectedProductItem;
        //    set => SetProperty(ref _selectedProductItem, value);
        //}

        //private DelegateCommand<object> _productValueChangedCommand;


        //public DelegateCommand<object> ProductValueChangedCommand =>
        //    _productValueChangedCommand ?? (_productValueChangedCommand = new DelegateCommand<object>((t) => ProductValueChangedCmd(t)));

        //private void ProductValueChangedCmd(object value)
        //{


        //}
        #endregion
    }
}

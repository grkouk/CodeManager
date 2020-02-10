using Prism.Commands;
using Prism.Mvvm;
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
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.CodeManager.ViewModels
{
    public class ShopToolsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;

        public ShopToolsPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IDataSource dataSource) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataSource = dataSource;
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (ShopList == null)
            {
                LoadShops();
                SelectedShopIndex = 0;
            }
            if (NopItems == null)
            {
                await RefreshDataAsync();
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
        #region Product Selected
        private bool _productSelected = false;
        public bool ProductSelected
        {
            get => _productSelected;
            set => SetProperty(ref _productSelected, value);
        }
        #endregion
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
            if (!String.IsNullOrEmpty(_selectedShopId))
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
        #region AutoCompleteSelection Chanded

        private DelegateCommand<object> _acSelectionChangedCmd;

        public DelegateCommand<object> AcSelectionChangedCmd =>
            _acSelectionChangedCmd ?? (_acSelectionChangedCmd = new DelegateCommand<object>(async (t) => await AcSelectionChangedImpl(t)));

        private async Task AcSelectionChangedImpl(object value)
        {
#if DEBUG
            Debug.WriteLine("AA AcSelectionChangedImpl");
#endif
            if (value != null)
            {
                ProductSelected = SelectedProductItem != null;
                if (value is ProductListDto)
                {
                    _selectedProductId = (value as ProductListDto).Id;
                    _productAttrCombList = await GetShopProductAttrCombinationsAsync();
                    NumberOfProductAttrCombinations = _productAttrCombList?.Count() ?? 0;
#if DEBUG
                    try
                    {
                        var debugMessage = $"AA AcSelectionChangedImpl Parameter value is {(value as ProductListDto).Name}";
                        Debug.WriteLine(debugMessage);
                        debugMessage = $"AA AcSelectionChangedImpl Selected index is {_selectedProductId}";
                        Debug.WriteLine(debugMessage);
                        if (_selectedProductItem is null)
                        {
                            debugMessage = $"AA AcSelectionChangedImpl Selected Item is null";
                        }
                        else
                        {
                            debugMessage = $"AA AcSelectionChangedImpl Selected Item is {_selectedProductItem.Name}";
                        }
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
        #region AutoCompleteValue Changed

        private DelegateCommand<object> _acValueChangedCmd;

        public DelegateCommand<object> AcValueChangedCmd =>
            _acValueChangedCmd ?? (_acValueChangedCmd = new DelegateCommand<object>((t) => AcValueChangedImpl(t)));

        private void AcValueChangedImpl(object value)
        {
#if DEBUG
            Debug.WriteLine("AcValueChangedImpl");
#endif
            if (value != null)
            {

                if (value is string)
                {
                    if (!string.IsNullOrEmpty((string)value))
                    {
                        ProductSelected = SelectedProductItem != null;
#if DEBUG
                        try
                        {
                            var debugMessage = $"AcValueChangedImpl parameter value is {value}";
                            Debug.WriteLine(debugMessage);
                            debugMessage = $"AcValueChangedImpl Selected index is {_selectedProductId}";
                            Debug.WriteLine(debugMessage);
                            if (_selectedProductItem is null)
                            {
                                debugMessage = $"AcValueChangedImpl Selected Item is null";
                            }
                            else
                            {
                                debugMessage = $"AcValueChangedImpl Selected Item is {_selectedProductItem.Name}";
                            }
                            Debug.WriteLine(debugMessage);
                        }
                        catch
                        {

                        }
#endif
                    }
                    else //value is null or empty
                    {
                        ProductSelected = false;

#if DEBUG
                        try
                        {
                            var debugMessage = $"AcValueChangedImpl parameter value is null or empty string";
                            Debug.WriteLine(debugMessage);
                            debugMessage = $"AcValueChangedImpl Selected index is {_selectedProductId}";
                            Debug.WriteLine(debugMessage);
                            if (_selectedProductItem is null)
                            {
                                debugMessage = $"AcValueChangedImpl Selected Item is null";
                            }
                            else
                            {
                                debugMessage = $"AcValueChangedImpl Selected Item is {_selectedProductItem.Name}";
                            }
                            Debug.WriteLine(debugMessage);
                        }
                        catch
                        {

                        }
#endif  
                    }

                }

            }


        }

        #endregion
        #endregion

        #region NumberOfAttributeCombinations
        private int _numberOfProductAttrCombinations;
        public int NumberOfProductAttrCombinations
        {
            get => _numberOfProductAttrCombinations;
            set => SetProperty(ref _numberOfProductAttrCombinations, value);
        }


        #endregion

        #region ProductAttributeCombinations

        private IEnumerable<ProductAttrCombinationDto> _productAttrCombList;
        private async Task<IEnumerable<ProductAttrCombinationDto>> GetShopProductAttrCombinationsAsync()
        {
            if (!String.IsNullOrEmpty(_selectedShopId)&&_selectedProductId>0)
            {
                if (Int32.TryParse(_selectedShopId, out int shopId))
                {
                    return await _dataSource.GetShopProductAttrCombinations(shopId, _selectedProductId);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
        #endregion
    }
}

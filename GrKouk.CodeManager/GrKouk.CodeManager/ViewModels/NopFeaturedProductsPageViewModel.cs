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
using CollectionViewChangedEventArgs=Xamarin.Forms.SelectionChangedEventArgs;

namespace GrKouk.CodeManager.ViewModels
{
    public class NopFeaturedProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;
        public NopFeaturedProductsPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IDataSource dataSource)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataSource = dataSource;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (ShopList == null)
            {
                LoadShops();
                SelectedShopIndex = 0;
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
                        RefreshFeaturedProductsCommand.Execute();

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
        #region SelectionChanged

        private DelegateCommand<object> _selProdChangedCommand;
        public DelegateCommand<object> SelectedProdChangedCommand =>
            _selProdChangedCommand ?? (_selProdChangedCommand = new DelegateCommand<object>((t) => SelectedProdChangedCmd(t)));

        private void SelectedProdChangedCmd(object value)
        {
#if DEBUG
            Debug.WriteLine("SelectedProdChangedCmd");
#endif
            ProductSelected = false;
            if (value!=null)
            {

                var selectionList = value as CollectionViewChangedEventArgs;
                if (selectionList?.CurrentSelection.Count > 0)
                {
                    ProductSelected = true;
                }

            }
        }

        #endregion
        #region Product Selected
        private bool _productSelected = false;
        public bool ProductSelected
        {
            get => _productSelected;
            set => SetProperty(ref _productSelected, value);
        }
        

        #endregion
        private int _selectedProductId;
        private ObservableCollection<ProductListDto> _featuredProductList;

        public ObservableCollection<ProductListDto> FeaturedProductList
        {
            get => _featuredProductList;
            set => SetProperty(ref _featuredProductList, value);
        }
        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshFeaturedProductsCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () => await RefreshFeaturedProductsDataCommand(), () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        async Task RefreshFeaturedProductsDataCommand()
        {
            await RefreshFeaturedProductsListAsync();
        }
        private async Task RefreshFeaturedProductsListAsync()
        {
            IsBusy = true;
            try
            {
                var nnItems = new ObservableCollection<ProductListDto>();
                var npItems = await GetNopFeaturedProductsAsync();
                if (npItems != null)
                {
                    foreach (var item in npItems)
                    {
                        nnItems.Add(item);
                    }
                }
                FeaturedProductList = nnItems;
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
        private async Task<IEnumerable<ProductListDto>> GetNopFeaturedProductsAsync()
        {
            if (!String.IsNullOrEmpty(_selectedShopId))
            {
                int.TryParse(_selectedShopId, out int sId);
                return await _dataSource.GetShopFeaturedProductList(sId);
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
        private ObservableCollection<object> _selectedProducts=new ObservableCollection<object>();
        public ObservableCollection<object> SelectedProducts
        {
            get => _selectedProducts;
            set => SetProperty(ref _selectedProducts, value);
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
                //ProductSelected = SelectedProductItem != null;
                if (value is ProductListDto)
                {
                    _selectedProductId = (value as ProductListDto).Id;

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
                        //ProductSelected = SelectedProductItem != null;
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
                       // ProductSelected = false;

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
        #region UnCheckFeaturedProductCommand

       
        private DelegateCommand _uncheckFeaturedProductCmd;

        public DelegateCommand UncheckFeaturedProductCmd =>
            _uncheckFeaturedProductCmd ?? (_uncheckFeaturedProductCmd = new DelegateCommand(async () => await UncheckFeaturedProductImpl(), () => ProductSelected)).ObservesProperty(() => ProductSelected);
        private async Task UncheckFeaturedProductImpl()
        {
            try
            {
                if (Int32.TryParse(_selectedShopId, out int shopId))
                {
                    string selProdIds =string.Empty ;
                    int i = 0;
                    if (SelectedProducts!=null)
                    {
                        foreach (ProductListDto item in SelectedProducts)
                        {
                            if (i > 0)
                            {
                                selProdIds += ",";
                            }
                            selProdIds += item.Id;
                            i++;
                        }
                    }
                    
                    var retResponse = await _dataSource.UncheckFeaturedProductsAsync(shopId, selProdIds);
                    var msg = $"Should Update {retResponse.ToAffectCount} {Environment.NewLine}Actually updated {retResponse.AffectedCount}{Environment.NewLine}Message={retResponse.Message}";
                    var tasks = new Task[] {
                        RefreshFeaturedProductsDataCommand(),
                        _dialogService.DisplayAlertAsync("Info", msg, "Ok")
                    };
                    await Task.WhenAll(tasks);
                    ProductSelected = false;
                   
                }

            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
                var msg = "This task was cancelled (timed out)";
                await _dialogService.DisplayAlertAsync("Task Cancelled", msg, "Ok");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
        }
        #endregion
    }
}

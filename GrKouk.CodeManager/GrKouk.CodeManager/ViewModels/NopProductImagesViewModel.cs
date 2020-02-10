using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Helpers;
using GrKouk.CodeManager.Models;
using GrKouk.CodeManager.Services;
using GrKouk.Shared.Core;
using GrKouk.Shared.Definitions;
using GrKouk.Shared.Mobile.Dtos;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.CodeManager.ViewModels
{
    public class NopProductImagesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;
        public NopProductImagesViewModel(INavigationService navigationService, IPageDialogService dialogService, IDataSource dataSource) : base(navigationService)
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
                SelectedShopIndex = 0;
            }
        }
        #region Picture text suggestion properties
        private string _pictureTitleTextSuggestion;
        public string PictureTitleTextSuggestion
        {
            get => _pictureTitleTextSuggestion;
            set => SetProperty(ref _pictureTitleTextSuggestion, value);
        }

        private string _pictureAltTextSuggestion;
        public string PictureAltTextSuggestion
        {
            get => _pictureAltTextSuggestion;
            set => SetProperty(ref _pictureAltTextSuggestion, value);
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
        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion
        #region CreateImages

        private void LoadMimeTypes()
        {
            var items = new List<ListItemDto>
            {
                new ListItemDto {ItemId = 0, ItemName = "image/jpeg", ItemCode = "jpeg"},
                new ListItemDto {ItemId = 0, ItemName = "image/jpg", ItemCode = "jpg"},
                new ListItemDto {ItemId = 0, ItemName = "image/png", ItemCode = "png"}
            };
            var cItems = new ObservableCollection<ListItemDto>();
            foreach (var item in items)
            {
                cItems.Add(item);
            }

            MimeTypes = cItems;
        }
        private ObservableCollection<ProductImageDto> _imageList;
        public ObservableCollection<ProductImageDto> ImageList
        {
            get => _imageList;
            set => SetProperty(ref _imageList, value);
        }
        private ObservableCollection<ListItemDto> _mimeTypes;
        public ObservableCollection<ListItemDto> MimeTypes
        {
            get => _mimeTypes;
            set => SetProperty(ref _mimeTypes, value);
        }
        private int _numberOfImages = 1;
        public int NumberOfImages
        {
            get => _numberOfImages;
            set => SetProperty(ref _numberOfImages, value);
        }
        private DelegateCommand _createImagesCmd;

        public DelegateCommand CreateImagesCmd =>
            _createImagesCmd ?? (_createImagesCmd = new DelegateCommand(() => CreateImagesImpl(), () => ProductSelected)).ObservesProperty(() => ProductSelected);
        private void CreateImagesImpl()
        {
            try
            {
                LoadMimeTypes();
                var nnItems = new ObservableCollection<ProductImageDto>();
                if (SelectedProductItem != null)
                {

                }
                for (int i = 0; i < _numberOfImages; i++)
                {
                    var item = new ProductImageDto
                    {
                        PictureId = 0,
                        MimeType = "jpeg",
                        SeoFilename = !String.IsNullOrEmpty(PrimarySlugText) ? PrimarySlugText : string.Empty,
                        AltAttribute = !string.IsNullOrEmpty(PictureAltTextSuggestion) ? PictureAltTextSuggestion : string.Empty,
                        TitleAttribute = !string.IsNullOrEmpty(PictureTitleTextSuggestion) ? PictureTitleTextSuggestion : string.Empty,
                        DisplayOrder = 32
                    };
                    nnItems.Add(item);
                }
                //LoadMimeTypes();
                ImageList = nnItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
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
        #region PrimarySlug
        private string _primarySlugText;
        public string PrimarySlugText
        {
            get => _primarySlugText;
            set => SetProperty(ref _primarySlugText, value);
        }

        private async Task<string> GetProductPrimarySlugAsync(string shop, int productId)
        {
            var primarySlug = await _dataSource.GetNopShopPrimaryProductSlug(shop, productId);
            if (primarySlug != null)
            {
                return primarySlug.ItemName;
            }

            return null;
        }
        #endregion
        #region Slugs

        private string _selectedSlugText;
        public string SelectedSlugText
        {
            get => _selectedSlugText;
            set => SetProperty(ref _selectedSlugText, value);
        }
        private string _selectedSlugId;

        private ObservableCollection<ListItemDto> _slugList;

        public ObservableCollection<ListItemDto> SlugList
        {
            get => _slugList;
            set => SetProperty(ref _slugList, value);
        }
        private int _selectedSlugIndex;
        public int SelectedSlugIndex
        {
            get => _selectedSlugIndex;
            set => SetProperty(ref _selectedSlugIndex, value);
        }
        private ListItemDto _selectedSlug;
        public ListItemDto SelectedSlug { get => _selectedSlug; set => SetProperty(ref _selectedSlug, value); }

        #region SlugValueChanged

        private DelegateCommand<object> _selectedSlugIndexChangedCommand;
        public DelegateCommand<object> SelectedSlugIndexChangedCommand =>
            _selectedSlugIndexChangedCommand ?? (_selectedSlugIndexChangedCommand = new DelegateCommand<object>((t) => SelectedSlugIndexChangedCmd(t)));

        private void SelectedSlugIndexChangedCmd(object value)
        {
#if DEBUG
            Debug.WriteLine("SelectedSlugIndexChangedCmd");
#endif
            if (value != null)
            {

                if (value is ListItemDto)
                {
                    _selectedSlugId = (value as ListItemDto).ItemCode;
#if DEBUG
                    try
                    {
                        var debugMessage = $"Selected index is {_selectedSlugIndex} item value is {_selectedSlugId}";
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

        private async Task RefreshSlugsAsync()
        {
            IsBusy = true;
            try
            {
                var nnItems = new ObservableCollection<ListItemDto>();
                var npItems = await GetNopProductSlugsAsync();
                if (npItems != null)
                {
                    foreach (var item in npItems)
                    {

                        nnItems.Add(item);

                    }
                }
                SlugList = nnItems;
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

        private async Task<IEnumerable<ListItemDto>> GetNopProductSlugsAsync()
        {
            if (!String.IsNullOrEmpty(_selectedShopId))
            {
                return await _dataSource.GetNopShopProductSlugsListAsync(_selectedShopId, _selectedProductId);
            }

            return null;
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
                    //get slugs for selected productId
                    PrimarySlugText = await GetProductPrimarySlugAsync(_selectedShopId, _selectedProductId);
                    PictureTitleTextSuggestion = (value as ProductListDto).Name;
                    PictureAltTextSuggestion = (value as ProductListDto).Name;
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
        #region AutoCompleteValue Chanded

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
                        PictureAltTextSuggestion = string.Empty;
                        PictureTitleTextSuggestion=String.Empty;
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

        #region Product Slugs
        private ObservableCollection<ProductListDto> _productSlugs;

        public ObservableCollection<ProductListDto> ProductSlugs
        {
            get => _productSlugs;
            set => SetProperty(ref _productSlugs, value);
        }


        #endregion
    }
}

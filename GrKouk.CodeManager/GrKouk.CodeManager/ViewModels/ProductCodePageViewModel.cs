using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Services;
using GrKouk.Shared.Mobile.Dtos;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductCodePageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;

        public ProductCodePageViewModel(INavigationService navigationService, IPageDialogService dialogService
        , IDataSource dataSource
            ) : base(navigationService)
        {
            _dialogService = dialogService;
            _dataSource = dataSource;
            Title = "Lookup a Code";
        }

        public string CodeLookup
        {
            get => _codeLookup;
            set => SetProperty(ref _codeLookup, value);
        }
        private string _codeLookup;
        
        private DelegateCommand _lookupCommand;
        
        public DelegateCommand LookupCommand =>
            _lookupCommand ?? (_lookupCommand = new DelegateCommand(async () => await LookupDataImpl()));
        private async Task LookupDataImpl()
        {
            try
            {
                if (_nopItems != null)
                {
                    _nopItems.Clear();
                    NopItems = _nopItems;
                }
                if (_erpItems != null)
                {
                    _erpItems.Clear();
                    ErpItems = _erpItems;
                }

                await RefreshDataAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
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
        private ObservableCollection<ProductCodeLookupDto> _nopItems;

        public ObservableCollection<ProductCodeLookupDto> NopItems
        {
            get => _nopItems;
            set => SetProperty(ref _nopItems, value) ;
        }
        private ObservableCollection<ProductListDto> _erpItems;

        public ObservableCollection<ProductListDto> ErpItems
        {
            get => _erpItems;
            set => SetProperty(ref _erpItems, value);
        }
        #region RefreshCommand

        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () => await RefreshDataCommand(), () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        async Task RefreshDataCommand()
        {
            await RefreshDataAsync();
        }

        #endregion
        private async Task RefreshDataAsync()
        {
            IsBusy = true;
            try
            {
                var nnItems = new ObservableCollection<ProductCodeLookupDto>();
               
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

                var eeItems = new ObservableCollection<ProductListDto>();
                var erItems = await GetItemsAsync();
                if (erItems != null)
                {
                    foreach (var item in erItems)
                    {
                        if (!String.IsNullOrEmpty(item.Code))
                        {
                            eeItems.Add(item);
                        }
                    }
                }

                ErpItems = eeItems;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");

                //throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<IEnumerable<ProductListDto>> GetItemsAsync()
        {
            return await _dataSource.GetCodesAsync(_codeLookup);
        }
        private async Task<IEnumerable<ProductCodeLookupDto>> GetNopItemsAsync()
        {
            return await _dataSource.GetNopCodesAsyncV2(_codeLookup);
        }
    }
}

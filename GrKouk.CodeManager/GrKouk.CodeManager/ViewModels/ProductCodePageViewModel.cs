using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;
using GrKouk.CodeManager.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductCodePageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly ICodeDataSource _dataSource;

        public ProductCodePageViewModel(INavigationService navigationService, IPageDialogService dialogService
        ,ICodeDataSource dataSource
            ) : base(navigationService)
        {
            _dialogService = dialogService;
            _dataSource = dataSource;
            Title = "Lookup a Code";
        }

        public string CodeLookup
        {
            get => _codeLookup;
            set =>  SetProperty(ref _codeLookup,value);
        }

        private DelegateCommand _lookupCommand;
        private string _codeLookup;

        public DelegateCommand LookupCommand =>
            _lookupCommand ?? (_lookupCommand = new DelegateCommand(async () => await LookupDataCommand()));
        private async Task LookupDataCommand()
        {
            try
            {
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
        private ObservableCollection<CodeDto> _itemsCollection;

        public ObservableCollection<CodeDto> ItemsCollection
        {
            get => _itemsCollection;
            set => SetProperty(ref _itemsCollection, value);
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
                if (ItemsCollection == null)
                {
                    ItemsCollection = new ObservableCollection<CodeDto>();
                }
                ItemsCollection.Clear();
                var items = await GetItemsAsync();
                foreach (var item in items)
                {
                    ItemsCollection.Add(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");

                //throw;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task<IEnumerable<CodeDto>> GetItemsAsync()
        {
            return await _dataSource.GetCodesAsync(_codeLookup);
        }
    }
}

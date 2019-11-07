using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;
using GrKouk.CodeManager.Services;
using GrKouk.InfoSystem.Dtos.MobileDtos;
using Prism.Navigation;
using Prism.Services;
using GrKouk.InfoSystem.Dtos.WebDtos;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;
        private readonly ICodeDataSource _codeSource;

        public ProductsPageViewModel(INavigationService navigationService, IPageDialogService dialogService
            ,IDataSource dataSource
            ,ICodeDataSource codeSource) : base(navigationService)
        {
            _dialogService = dialogService;
            _dataSource = dataSource;
            _codeSource = codeSource;
            Title = "Products Page";
        }

        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        private ObservableCollection<ProductListDto> _itemsCollection;

        public ObservableCollection<ProductListDto> ItemsCollection
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
            //if (IsBusy)
            //{
            //    return;
            //}

            IsBusy = true;
            try
            {
                if (ItemsCollection == null)
                {
                    ItemsCollection = new ObservableCollection<ProductListDto>();
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

        private async Task<IEnumerable<ProductListDto>> GetItemsAsync()
        {
          
            return await _dataSource.GetAllProductsAsync();
        }

        #region OnNavigatedTo

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (ItemsCollection == null)
            {

                await RefreshDataAsync();
            }

            if (parameters != null)
            {
                if (parameters.ContainsKey("RefreshView"))
                {
                    await RefreshDataAsync();
                }
            }
        }

        #endregion  
    }
}

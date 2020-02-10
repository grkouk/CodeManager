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


namespace GrKouk.CodeManager.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;
        

        public ProductsPageViewModel(INavigationService navigationService, IPageDialogService dialogService
            ,IDataSource dataSource
            ) : base(navigationService)
        {
            _dialogService = dialogService;
            _dataSource = dataSource;
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
                var itCol = new ObservableCollection<ProductListDto>();
                //if (ItemsCollection == null)
                //{
                //    ItemsCollection = new ObservableCollection<ProductListDto>();
                //}
                //ItemsCollection.Clear();
                var items = await GetItemsAsync();
                if (items !=null)
                {
                    foreach (var item in items)
                    {
                        itCol.Add(item);
                    }

                    ItemsCollection = itCol;
                }
               
            }
            catch (Exception e)
            {
               await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");
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

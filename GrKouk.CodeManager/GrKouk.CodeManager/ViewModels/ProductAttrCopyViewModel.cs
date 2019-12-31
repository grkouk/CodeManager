using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GrKouk.CodeManager.Models;
using GrKouk.CodeManager.Services;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductAttrCopyViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;

        public ProductAttrCopyViewModel(INavigationService navigationService
            , IPageDialogService dialogService
            , IDataSource dataSource ) : base(navigationService)
        {
            _dialogService = dialogService;
            _dataSource = dataSource;
            Title = "Lookup a Code";
        }
        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        private ObservableCollection<ListItemDto> _productItems;
        public ObservableCollection<ListItemDto> ProductItems
        {
            get => _productItems;
            set => SetProperty(ref _productItems, value);
        }

        private ObservableCollection<ListItemDto> _attributeItems;
        public ObservableCollection<ListItemDto> AttributeItems
        {
            get => _attributeItems;
            set => SetProperty(ref _attributeItems, value);
        }
    }
}

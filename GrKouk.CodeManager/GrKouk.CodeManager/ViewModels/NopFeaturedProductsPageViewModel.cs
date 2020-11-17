using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using GrKouk.CodeManager.Services;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.CodeManager.ViewModels
{
    public class NopFeaturedProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataSource _dataSource;
        public NopFeaturedProductsPageViewModel()
        {

        }
    }
}

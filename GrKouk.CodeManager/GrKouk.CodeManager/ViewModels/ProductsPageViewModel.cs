using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;

        public ProductsPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            Title = "Settings Page";
        }
    }
}

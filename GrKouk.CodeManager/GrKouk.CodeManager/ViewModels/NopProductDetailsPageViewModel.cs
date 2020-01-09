using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrKouk.CodeManager.ViewModels
{
    public class NopProductDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public NopProductDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}

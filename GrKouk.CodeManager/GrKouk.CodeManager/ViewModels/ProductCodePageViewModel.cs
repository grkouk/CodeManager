using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductCodePageViewModel : ViewModelBase
    {
        public ProductCodePageViewModel(INavigationService navigationService) :base(navigationService)
        {
            Title = "Product Page";
        }
    }
}

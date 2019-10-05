using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        

        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogService
        ) : base(navigationService)
        {
            _dialogService = dialogService;
            Title = "Product Code Page";

            WebApiBaseAddress = Preferences.Get(Constants.WebApiBaseAddressKey, "Not Set");
        }

        private string _webApiBaseAddress;
        public string WebApiBaseAddress
        {
            get => _webApiBaseAddress;
            set => SetProperty(ref _webApiBaseAddress, value);  
        }


        private DelegateCommand _saveCommand;
       

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));
        private async Task SaveDataCommand()
        {
            try
            {
                Preferences.Set(Constants.WebApiBaseAddressKey, _webApiBaseAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
        }

    }
}

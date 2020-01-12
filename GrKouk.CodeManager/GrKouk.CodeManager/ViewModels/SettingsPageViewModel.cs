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
            Title = "Settings Page";

            WebApiBaseAddress = Preferences.Get(Constants.WebApiErpBaseAddressKey, "Not Set");
            WebApiNopBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "Not Set");

        }

        private string _webApiBaseAddress;
        public string WebApiBaseAddress
        {
            get => _webApiBaseAddress;
            set => SetProperty(ref _webApiBaseAddress, value);  
        }

        private string _webApiNopBaseAddress;
        public string WebApiNopBaseAddress
        {
            get => _webApiNopBaseAddress;
            set => SetProperty(ref _webApiNopBaseAddress, value);
        }
        
        private string _webSiteUrl;
        public string WebSiteUrl
        {
            get => _webSiteUrl;
            set => SetProperty(ref _webSiteUrl, value);
        }
        private string _campaignSource;
        public string CampaignSource
        {
            get => _campaignSource;
            set => SetProperty(ref _campaignSource, value);
        }
        private string _campaignMedium;
        public string CampaignMedium
        {
            get => _campaignMedium;
            set => SetProperty(ref _campaignMedium, value);
        }
        private string _campaignName;
        public string CampaignName
        {
            get => _campaignName;
            set => SetProperty(ref _campaignName, value);
        }

        private DelegateCommand _saveCommand;
        


        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));
        private async Task SaveDataCommand()
        {
            try
            {
                Preferences.Set(Constants.WebApiErpBaseAddressKey, _webApiBaseAddress);
                Preferences.Set(Constants.WebApiNopBaseAddressKey, _webApiNopBaseAddress);
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

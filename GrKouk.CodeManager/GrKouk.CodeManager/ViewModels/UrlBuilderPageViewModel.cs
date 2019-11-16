using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.CodeManager.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class UrlBuilderPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;

        public UrlBuilderPageViewModel(INavigationService navigationService
            , IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            Title = "Url Builder";
        }

        #region IsBusy

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Bindable Properties

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

        private string _generatedUrl;

        public string GeneratedUrl
        {
            get => _generatedUrl;
            set => SetProperty(ref _generatedUrl, value);
        }

        #endregion

        #region Create Url Command

        private DelegateCommand _createCommand;

        public DelegateCommand CreateUrlCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(async () => await CreateUrlCmd()));

        private async Task CreateUrlCmd()
        {
            try
            {
                await CreateUrlCmdAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
        }

        private async Task CreateUrlCmdAsync()
        {
            IsBusy = true;
            try
            {
                var finalUrl = "";
                finalUrl = $"{WebSiteUrl}?{CampaignSource}&{CampaignMedium}{CampaignName}";
                GeneratedUrl = finalUrl;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");

                //throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
        public bool HasText => !string.IsNullOrEmpty(GeneratedUrl);
        #region Clipboard
        private DelegateCommand _copyToClipCommand;

        public DelegateCommand CopyToClipCommand =>
            _copyToClipCommand ?? (_copyToClipCommand = new DelegateCommand(async () => await CopyToClipCmd()))
            .ObservesCanExecute(()=>HasText);

        private async Task CopyToClipCmd()
        {
            await Clipboard.SetTextAsync("");
        }
        #endregion
    }
}

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GrKouk.CodeManager.Helpers;
using GrKouk.Shared.Core;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Syncfusion.Data;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private string _lastProfile;

        #region SaveProfileCommand

        private DelegateCommand _saveProfileCmd;

        public DelegateCommand SaveProfileCmd => 
            _saveProfileCmd ?? (_saveProfileCmd = new DelegateCommand(()=>SaveProfileCmdImpl() ));

        private void SaveProfileCmdImpl()
        {
            string profName;
            if (!string.IsNullOrEmpty(_profileNameText))
            {
                KeyValuePair<string, string> setting;
                List<KeyValuePair<string, string>> settings=new List<KeyValuePair<string, string>>();
                setting=new KeyValuePair<string, string>( Constants.WebApiErpBaseAddressKey , _webApiBaseAddress);
                settings.Add(setting);
                setting = new KeyValuePair<string, string>(Constants.WebApiNopBaseAddressKey, _webApiNopBaseAddress);
                settings.Add(setting);
                string jsonSettings = JsonConvert.SerializeObject(settings);
                ObservableCollection<SettingsProfile> profiles = ProfileList;
                var spro = new SettingsProfile
                {
                    ProfileName = _profileNameText,
                    ProfileSettings = jsonSettings
                };
                profiles.Add(spro);

                string jsonProfiles = JsonConvert.SerializeObject(profiles);
                Preferences.Set("SavedApiProfiles", jsonProfiles);

            }
        }

        #endregion
        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogService
        ) : base(navigationService)
        {
            _dialogService = dialogService;
            Title = "Settings Page";

            WebApiBaseAddress = Preferences.Get(Constants.WebApiErpBaseAddressKey, "Not Set");
            WebApiNopBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "Not Set");
            
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
           

            if (ProfileList is null)
            {
               LoadSavedProfiles();
            }
        }

        #region SelectedProfileChanged

        private int _selectedProfileIndex;
        public int SelectedProfileIndex
        {
            get => _selectedProfileIndex;
            set => SetProperty(ref _selectedProfileIndex, value);
        }
        private SettingsProfile _selectedProfile;
        public SettingsProfile SelectedShop { get => _selectedProfile; set => SetProperty(ref _selectedProfile, value); }

        #region ProfileValueChanged

        private DelegateCommand<object> _selectedProfileIndexChangedCmd;
        public DelegateCommand<object> SelectedProfileIndexChangedCommand =>
            _selectedProfileIndexChangedCmd ?? (_selectedProfileIndexChangedCmd = new DelegateCommand<object>((t) => SelectedProfileIndexChangedImpl(t)));

        private void SelectedProfileIndexChangedImpl(object value)
        {
#if DEBUG
            Debug.WriteLine("SelectedProfileIndexChangedImpl");
#endif
            if (value != null)
            {

                if (value is string)
                {
                }

                if (value is SettingsProfile)
                {
                  var prName = (value as SettingsProfile).ProfileName;
                    

#if DEBUG
                    try
                    {
                        var debugMessage = $"Selected index is value is {prName}";
                        Debug.WriteLine(debugMessage);
                    }
                    catch
                    {

                    }
#endif
                }

            }
            else
            {

            }
        }

        #endregion
#endregion
        #region ProfileSelectControl

        private string _profileNameText;
        public string ProfileNameText
        {
            get => _profileNameText;
            set => SetProperty(ref _profileNameText, value);
        }
        #endregion
        #region ProfileList

        private void LoadSavedProfiles()
        {
            var sp = Preferences.Get("SavedApiProfiles", "#");
            if (sp=="#")
            {
                ProfileList = new ObservableCollection<SettingsProfile>();
            }
            else
            {
                var svList = JsonConvert.DeserializeObject<List<SettingsProfile>>(sp);
                var profList = new ObservableCollection<SettingsProfile>();
                foreach (var settingsProfile in svList)
                {
                    profList.Add( new SettingsProfile
                    {
                        ProfileName = settingsProfile.ProfileName,
                        ProfileSettings = settingsProfile.ProfileSettings
                    });
                }

                ProfileList = profList;
            }
        }
        private ObservableCollection<SettingsProfile> _profileList;

        public ObservableCollection<SettingsProfile> ProfileList
        {
            get => _profileList;
            set => SetProperty(ref _profileList, value);
        }
        #endregion
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

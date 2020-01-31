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

        #region Can Save Profile Property
        private bool _canSaveProfile = false;
        public bool CanSaveProfile
        {
            get => _canSaveProfile;
            set => SetProperty(ref _canSaveProfile, value);
        }

        #endregion
        #region Can Apply Profile Property
        private bool _canApplyProfile = false;
        public bool CanApplyProfile
        {
            get => _canApplyProfile;
            set => SetProperty(ref _canApplyProfile, value);
        }

        #endregion
        #region SaveProfileCommand

        private DelegateCommand _saveProfileCmd;

        public DelegateCommand SaveProfileCmd =>
            _saveProfileCmd ?? (_saveProfileCmd = new DelegateCommand(() => SaveProfileCmdImpl(), () => CanSaveProfile)).ObservesProperty(() => CanSaveProfile);

        private void SaveProfileCmdImpl()
        {
            string profName;
            if (!string.IsNullOrEmpty(_profileNameText))
            {
                KeyValuePair<string, string> setting;
                List<KeyValuePair<string, string>> settings = new List<KeyValuePair<string, string>>();
                setting = new KeyValuePair<string, string>(Constants.WebApiErpBaseAddressKey, _webApiBaseAddress);
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

        #region ApplyProfileCommand

        private DelegateCommand _applyProfileCmd;

        public DelegateCommand ApplyProfileCmd =>
            _applyProfileCmd ?? (_applyProfileCmd = new DelegateCommand(() => ApplyProfileCmdImpl(), () => CanApplyProfile)).ObservesProperty(() => CanApplyProfile);

        private void ApplyProfileCmdImpl()
        {
            if (SelectedProfile != null)
            {
                var profileSetings = SelectedProfile.ProfileSettings;

                var settings = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(profileSetings);
                foreach (var setting in settings)
                {
                    Debug.WriteLine("AA "+ setting.Key + "----->" + setting.Value);

                    switch (setting.Key)
                    {
                        case Constants.WebApiErpBaseAddressKey:
                            WebApiBaseAddress = setting.Value;
                            Preferences.Set(Constants.WebApiErpBaseAddressKey, setting.Value);
                            break;
                        case Constants.WebApiNopBaseAddressKey:
                            Preferences.Set(Constants.WebApiNopBaseAddressKey, setting.Value);
                            WebApiNopBaseAddress = setting.Value;
                            break;
                    }
                }
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
                SelectedProfileIndex = -1;
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
        public SettingsProfile SelectedProfile { get => _selectedProfile; set => SetProperty(ref _selectedProfile, value); }

        #region SelectedProfileChangedCommand

        private DelegateCommand<object> _selectedProfileIndexChangedCmd;
        public DelegateCommand<object> SelectedProfileIndexChangedCommand =>
            _selectedProfileIndexChangedCmd ?? (_selectedProfileIndexChangedCmd = new DelegateCommand<object>((t) => SelectedProfileIndexChangedImpl(t)));

        private void SelectedProfileIndexChangedImpl(object value)
        {
#if DEBUG
            Debug.WriteLine("AA SelectedProfileIndexChangedImpl");
#endif
            if (value != null)
            {
                if (value is SettingsProfile)
                {
                    var prName = (value as SettingsProfile).ProfileName;
                    CanSaveProfile = false;
                    CanApplyProfile = true;

#if DEBUG
                    try
                    {
                        var debugMessage = $"AA Selected index is value is {prName}";
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
        #region ProfileTextChangedCommand

        private DelegateCommand<object> _profileTextChangedCmd;
        public DelegateCommand<object> ProfileTextChangedCmd =>
            _profileTextChangedCmd ?? (_profileTextChangedCmd = new DelegateCommand<object>((t) => ProfileTextChangedImpl(t)));

        private void ProfileTextChangedImpl(object value)
        {
#if DEBUG
            Debug.WriteLine("AA ProfileTextChangedImpl");
            var dm = $"AA value={value}, SelectedProfile={SelectedProfile}, SelectedIndex={SelectedProfileIndex}";
            Debug.WriteLine(dm);
#endif
            if (value != null)
            {
                if (value is string)
                {
                    if (!string.IsNullOrEmpty((string)value))
                    {
                        CanSaveProfile = SelectedProfile == null ? true : false;
                        CanApplyProfile = SelectedProfile != null ? true : false;
#if DEBUG
                        try
                        {
                            var debugMessage = $"AA profile text value is {value}";
                            Debug.WriteLine(debugMessage);
                        }
                        catch
                        {

                        }
#endif
                    }
                    else
                    {
                        CanSaveProfile = false;
                        CanApplyProfile = false;
                    }
                }


            }
            else
            {
                CanSaveProfile = false;
                CanApplyProfile = false;
            }
        }

        #endregion
        #endregion
        #region ProfileList

        private void LoadSavedProfiles()
        {
            var sp = Preferences.Get("SavedApiProfiles", "#");
            if (sp == "#")
            {
                ProfileList = new ObservableCollection<SettingsProfile>();
            }
            else
            {
                var svList = JsonConvert.DeserializeObject<List<SettingsProfile>>(sp);
                var profList = new ObservableCollection<SettingsProfile>();
                foreach (var settingsProfile in svList)
                {
                    profList.Add(new SettingsProfile
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

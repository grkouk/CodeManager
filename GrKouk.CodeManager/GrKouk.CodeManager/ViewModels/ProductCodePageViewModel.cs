﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.ViewModels
{
    public class ProductCodePageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;

        public ProductCodePageViewModel(INavigationService navigationService, IPageDialogService dialogService
            ) : base(navigationService)
        {
            _dialogService = dialogService;
            Title = "Product Code Page";
            string myValue = Preferences.Get("mySetting", "default_value");
            TestSetting = myValue;
        }

        public string TestSetting
        {
            get => _testSetting;
            set => SetProperty(ref _testSetting, value);
        }

        private string _testSetting;

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
             _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));
        private async Task SaveDataCommand()
        {
            try
            {
                Preferences.Set("mySetting", _testSetting);
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

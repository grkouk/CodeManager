using Syncfusion.SfDataGrid.XForms.UWP;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Syncfusion.SfPicker.XForms.UWP;

namespace GrKouk.CodeManager.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ5NTAzQDMxMzgyZTMzMmUzMFhlWkFjTHp4U25PUUpmOVVVVVc3MDArcUNiTFQxQmE3bHhKWFZoQjA4Wkk9;MzQ5NTA0QDMxMzgyZTMzMmUzMGlZUnBFL2crcVJlZUFYK2VteHBNZ0czZlljYmoyeFFRQmZzd3huMmFxcDA9");
            SfPickerRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            this.InitializeComponent();
            SfDataGridRenderer.Init();

            LoadApplication(new GrKouk.CodeManager.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

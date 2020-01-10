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

namespace GrKouk.CodeManager.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk0MDUyQDMxMzcyZTM0MmUzMEQwczdzaGFzZ3pqN25QamprWVRQc3Zyb0xLMDlVMEdGYVJnYkQzc3NxZmM9");
            this.InitializeComponent();

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

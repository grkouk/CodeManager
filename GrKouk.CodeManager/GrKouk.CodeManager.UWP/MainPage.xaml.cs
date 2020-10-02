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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjUzODczQDMxMzgyZTMxMmUzMG5UNHArR2l3MlJYaklGMlI5aTJXVnRuNnJuN1JLQVk5V1lUV0xaTDVxeGM9;MjUzODc0QDMxMzgyZTMxMmUzMFd6SVAveHB3cVZBQ0RsMUM3MXlhL0JKWjlUbU00R1ladTRlN2RsbGJXWmM9;MjUzODc1QDMxMzgyZTMxMmUzMGVIR01rLzlHYWsvUVlIYXJoSzF5RWQ3dnBxdEtmYjB2YTZRVHROQ0VnRlE9;MjUzODc2QDMxMzgyZTMxMmUzMGJ3aEw0RTBFWDBLUjFxcWIzQk0yRlNEaFdpWExYc2s4aWJIbmFxeE1aNW89;MjUzODc3QDMxMzgyZTMxMmUzMFBiSjkzSHVqMVVhRUNETmE0V1hTZG9PTTF5NDBQYTJicUVXQ2V0WXJRVlE9;MjUzODc4QDMxMzgyZTMxMmUzMGVPS0JHRmlTaTVYRm1lVWdsdTd0YnhuaHVhUnIvblM1OGt4R3g3N0o5Q289;MjUzODc5QDMxMzgyZTMxMmUzMGJuS3REbjJwV3haSWpyb29wT0ZzL2U2bXJmZ0NDaWs2R20xNERtNHVqQ1U9;MjUzODgwQDMxMzgyZTMxMmUzMEJLc2NUWGZjUC84Yms3TDJFdm9iY1BCeTJESDlYVkZvbDVsazFmUTlEYXc9;MjUzODgxQDMxMzgyZTMxMmUzMG15d2c3dXUwZEdEakhyUlMyZ0FYdGRPWHZKM05aNHdXeEN6amVVdU1ralU9;NT8mJyc2IWhia31ifWN9Z2FoYmF8YGJ8ampqanNiYmlmamlmanMDHmg0Njg8JjgTOzwnPjI6P30wPD4=;MjUzODgyQDMxMzgyZTMxMmUzMEJmeXlUUlFUelg0Wnpsa1VCS0FZaDllQkJoSzF4TWp4NVRDbFZIZDNTTWM9");
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

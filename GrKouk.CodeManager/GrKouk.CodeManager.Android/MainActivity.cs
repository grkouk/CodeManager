using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Prism;
using Prism.Ioc;

namespace GrKouk.CodeManager.Droid
{
    [Activity(Label = "GrKouk.CodeManager", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ5NTAzQDMxMzgyZTMzMmUzMFhlWkFjTHp4U25PUUpmOVVVVVc3MDArcUNiTFQxQmE3bHhKWFZoQjA4Wkk9;MzQ5NTA0QDMxMzgyZTMzMmUzMGlZUnBFL2crcVJlZUFYK2VteHBNZ0czZlljYmoyeFFRQmZzd3huMmFxcDA9");
            base.OnCreate(bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true );
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle
            LoadApplication(new App(new AndroidInitializer()));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}


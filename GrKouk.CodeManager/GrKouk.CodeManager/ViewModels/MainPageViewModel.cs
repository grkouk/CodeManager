using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrKouk.CodeManager.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GrKouk.CodeManager.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IDataSource _ds;
        public MainPageViewModel(INavigationService navigationService, IDataSource ds)
            : base(navigationService)
        {
            var vs = AppInfo.VersionString;
            Title = $"Main Page version {vs}";
            _ds = ds;
        }
        private string _backendVersion;
        public string BackendVersion { get => _backendVersion; set => _backendVersion = value; }
        private DelegateCommand _mediaCommand;
        private string _photoPath;
        public DelegateCommand MediaCommand =>
            _mediaCommand ?? (_mediaCommand = new DelegateCommand(async () => await TestComImpl()));
        private async Task TestComImpl()
        {
            try
            {
                var customFileType =
                    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // or general UTType values
                        { DevicePlatform.Android, new[] { "application/image" } },
                        { DevicePlatform.UWP, new[] { ".jpg", ".png" } },
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // or general UTType values
                    });
                var options = new PickOptions
                {
                    PickerTitle = "Please select a comic file",
                    FileTypes = customFileType,
                };
                var r = await PickAndShow(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"test THREW: {ex.Message}");
            }
        }

        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {_photoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        private async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                _photoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            _photoPath = newFile;
        }

        private string _testFileName;
        private ImageSource _image;
        private DelegateCommand _getApiInfoCommand;
        public DelegateCommand GetApiInfoCommand =>
            _getApiInfoCommand ?? (_getApiInfoCommand = new DelegateCommand(async () => await getApiInfoComImpl()));

        private async Task getApiInfoComImpl()
        {

        }
        private async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    _testFileName = $"File Name: {result.FileName}";
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();
                        _image = ImageSource.FromStream(() => stream);

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
                throw new Exception(ex.Message);
            }
        }

    }
}

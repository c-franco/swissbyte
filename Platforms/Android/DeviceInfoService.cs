using swissbyte.Platforms.Android;
using swissbyte.Interfaces;
using Android.OS;
using Microsoft.Maui.ApplicationModel;

[assembly: Dependency(typeof(DeviceInfoService))]
namespace swissbyte.Platforms.Android
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public string GetAndroidVersion() => Build.VERSION.Release;

        public int GetSdkVersion() => (int)Build.VERSION.SdkInt;

        public string GetManufacturer() => Build.Manufacturer;

        public string GetModel() => Build.Model;

        public string GetDeviceName() => Build.Device;
    }
}

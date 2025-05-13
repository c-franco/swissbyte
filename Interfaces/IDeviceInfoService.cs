namespace swissbyte.Interfaces
{
    public interface IDeviceInfoService
    {
        string GetAndroidVersion();
        int GetSdkVersion();
        string GetManufacturer();
        string GetModel();
        string GetDeviceName();
    }
}

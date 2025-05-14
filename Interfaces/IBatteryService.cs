namespace swissbyte.Interfaces
{
    public interface IBatteryService
    {
        string GetBatteryHealth();
        string GetBatteryType();
        string GetTemperature();
        string GetVoltage();
    }
}
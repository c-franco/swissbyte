using swissbyte.Interfaces;

namespace swissbyte.Pages.SystemInfo
{
    public partial class SystemInfoPage : ContentPage
    {
        private SystemInfoViewModel _viewModel;
        private IBatteryService _batteryHealthService;
        private IDeviceInfoService _deviceInfoService;

        public SystemInfoPage()
        {
            InitializeComponent();

            _batteryHealthService = Application.Current?.Handler.MauiContext?.Services.GetService<IBatteryService>();
            _deviceInfoService = Application.Current?.Handler.MauiContext?.Services.GetService<IDeviceInfoService>();
            _viewModel = new SystemInfoViewModel();
            BindingContext = _viewModel;

            GetSystemInfo();
        }

        private void GetSystemInfo()
        {
            string batteryHealth = GetBatteryHealth();
            _viewModel.UpdateBatteryHealth(batteryHealth);

            string batteryState = GetBatteryState();
            _viewModel.UpdateBatteryState(batteryState);

            string powerSource = GetPowerSource();
            _viewModel.UpdatePowerSource(powerSource);

            string batteryLevel = GetChargeLevel();
            _viewModel.UpdateChargeLevel(batteryLevel);

            string iconName = GetIconName(batteryLevel);
            _viewModel.UpdateIconName(iconName);

            string batteryType = GetBatteryType();
            _viewModel.UpdateBatteryType(batteryType);

            string temperature = GetTemperature();
            _viewModel.UpdateTemperature(temperature);

            string voltage = GetVoltage();
            _viewModel.UpdateVoltage(voltage);

            var version = _deviceInfoService?.GetAndroidVersion();
            _viewModel.UpdateAndroidVersion(version);

            var manufacturer = _deviceInfoService?.GetManufacturer();
            var model = _deviceInfoService?.GetModel();
            _viewModel.UpdateModel(manufacturer + " " + model);
        }

        private string GetBatteryHealth()
        {
            var batteryHealth = _batteryHealthService?.GetBatteryHealth();

            return batteryHealth ?? "No disponible";
        }

        private string GetBatteryState()
        {
            int batteryState = (int) Battery.State;
            string esBatteryState = string.Empty;

            switch(batteryState)
            {
                case 0:
                    esBatteryState = "Desconocido";
                    break;
                case 1:
                    esBatteryState = "Cargando";
                    break;
                case 2:
                    esBatteryState = "Descargando";
                    break;
                case 3:
                    esBatteryState = "Llena";
                    break;
                case 4:
                    esBatteryState = "Descargando";
                    break;
                case 5:
                    esBatteryState = "No presente";
                    break;
                default:
                    esBatteryState = "Desconocido";
                    break;
            }

            return esBatteryState ?? "No disponible";
        }

        private string GetPowerSource()
        {
            int powerSource = (int) Battery.PowerSource;
            string espowerSource = string.Empty;

            switch (powerSource)
            {
                case 0:
                    espowerSource = "Desconocido";
                    break;
                case 1:
                    espowerSource = "Batería";
                    break;
                case 2:
                    espowerSource = "Cargador";
                    break;
                case 3:
                    espowerSource = "USB";
                    break;
                case 4:
                    espowerSource = "Inalámbrico";
                    break;
                default:
                    espowerSource = "Desconocido";
                    break;
            }

            return espowerSource ?? "No disponible";
        }

        private string GetChargeLevel()
        {
            double batteryLevel = Battery.ChargeLevel * 100;

            return batteryLevel.ToString() + "%" ?? "No disponible";
        }

        private string GetIconName(string batteryLevel)
        {
            if (!batteryLevel.EndsWith("%"))
                return "battery_half.svg";

            if (!int.TryParse(batteryLevel.TrimEnd('%'), out int level))
                return "battery_half.svg";

            if (level >= 90)
                return "battery_full.svg";
            else if (level >= 60)
                return "battery_three_quarters.svg";
            else if (level >= 30)
                return "battery_half.svg";
            else if (level >= 10)
                return "battery_quarter.svg";
            else
                return "battery_empty.svg";
        }

        private string GetBatteryType()
        {
            var batteryType = _batteryHealthService?.GetBatteryType();

            return batteryType ?? "No disponible";
        }

        private string GetTemperature()
        {
            var temperature = _batteryHealthService?.GetTemperature();

            return temperature ?? "No disponible";
        }

        private string GetVoltage()
        {
            var voltage = _batteryHealthService?.GetVoltage();

            return voltage ?? "No disponible";
        }
    }
}

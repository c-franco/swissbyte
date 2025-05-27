using swissbyte.Config;
using swissbyte.Interfaces;
using swissbyte.ViewModels;

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

            return batteryHealth ?? Constantes.NoDisponible;
        }

        private string GetBatteryState()
        {
            int batteryState = (int) Battery.State;
            string esBatteryState = string.Empty;

            switch(batteryState)
            {
                case 0:
                    esBatteryState = Constantes.Desconocido;
                    break;
                case 1:
                    esBatteryState = Constantes.BatteryState_Cargando;
                    break;
                case 2:
                    esBatteryState = Constantes.BatteryState_Descargando;
                    break;
                case 3:
                    esBatteryState = Constantes.BatteryState_Llena;
                    break;
                case 4:
                    esBatteryState = Constantes.BatteryState_Descargando;
                    break;
                case 5:
                    esBatteryState = Constantes.BatteryState_NoPresente;
                    break;
                default:
                    esBatteryState = Constantes.Desconocido;
                    break;
            }

            return esBatteryState ?? Constantes.NoDisponible;
        }

        private string GetPowerSource()
        {
            int powerSource = (int) Battery.PowerSource;
            string esPowerSource = string.Empty;

            switch (powerSource)
            {
                case 0:
                    esPowerSource = Constantes.Desconocido;
                    break;
                case 1:
                    esPowerSource = Constantes.PowerSource_Bateria;
                    break;
                case 2:
                    esPowerSource = Constantes.PowerSource_Cargador;
                    break;
                case 3:
                    esPowerSource = Constantes.PowerSource_USB;
                    break;
                case 4:
                    esPowerSource = Constantes.PowerSource_Inalambrico;
                    break;
                default:
                    esPowerSource = Constantes.Desconocido;
                    break;
            }

            return esPowerSource ?? Constantes.NoDisponible;
        }

        private string GetChargeLevel()
        {
            double batteryLevel = Battery.ChargeLevel * 100;
            return $"{batteryLevel:F0}%" ?? Constantes.NoDisponible;
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

            return batteryType ?? Constantes.NoDisponible;
        }

        private string GetTemperature()
        {
            var temperature = _batteryHealthService?.GetTemperature();

            return temperature ?? Constantes.NoDisponible;
        }

        private string GetVoltage()
        {
            var voltage = _batteryHealthService?.GetVoltage();

            return voltage ?? Constantes.NoDisponible;
        }
    }
}

using System.ComponentModel;

namespace swissbyte.ViewModels
{
    public class SystemInfoViewModel : INotifyPropertyChanged
    {
        private string _batteryHealth;
        private string _chargeLevel;
        private string _batteryState;
        private string _powerSource;
        private string _iconName;
        private string _batteryType;
        private string _temperature;
        private string _voltage;
        private string _androidVersion;
        private string _model;

        public string IconName
        {
            get => _iconName;
            set
            {
                _iconName = value;
                OnPropertyChanged(nameof(IconName));
            }
        }

        public string BatteryHealth
        {
            get => _batteryHealth;
            set
            {
                if (_batteryHealth != value)
                {
                    _batteryHealth = value;
                    OnPropertyChanged(nameof(BatteryHealth));
                }
            }
        }

        public string ChargeLevel
        {
            get => _chargeLevel;
            set
            {
                if (_chargeLevel != value)
                {
                    _chargeLevel = value;
                    OnPropertyChanged(nameof(ChargeLevel));
                }
            }
        }

        public string BatteryState
        {
            get => _batteryState;
            set
            {
                if (_batteryState != value)
                {
                    _batteryState = value;
                    OnPropertyChanged(nameof(BatteryState));
                }
            }
        }

        public string PowerSource
        {
            get => _powerSource;
            set
            {
                if (_powerSource != value)
                {
                    _powerSource = value;
                    OnPropertyChanged(nameof(PowerSource));
                }
            }
        }

        public string BatteryType
        {
            get => _batteryType;
            set
            {
                if (_batteryType != value)
                {
                    _batteryType = value;
                    OnPropertyChanged(nameof(BatteryType));
                }
            }
        }

        public string Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        public string Voltage
        {
            get => _voltage;
            set
            {
                if (_voltage != value)
                {
                    _voltage = value;
                    OnPropertyChanged(nameof(Voltage));
                }
            }
        }

        public string AndroidVersion
        {
            get => _androidVersion;
            set
            {
                if (_androidVersion != value)
                {
                    _androidVersion = value;
                    OnPropertyChanged(nameof(AndroidVersion));
                }
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged(nameof(Model));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateBatteryHealth(string batteryHealth)
        {
            BatteryHealth = batteryHealth;
        }

        public void UpdateChargeLevel(string chargeLevel)
        {
            ChargeLevel = chargeLevel;
        }

        public void UpdateBatteryState(string batteryState)
        {
            BatteryState = batteryState;
        }

        public void UpdatePowerSource(string powerSource)
        {
            PowerSource = powerSource;
        }

        public void UpdateIconName(string iconName)
        {
            IconName = iconName;
        }

        public void UpdateBatteryType(string batteryType)
        {
            BatteryType = batteryType;
        }

        public void UpdateTemperature(string temperature)
        {
            Temperature = temperature;
        }

        public void UpdateVoltage(string voltage)
        {
            Voltage = voltage;
        }

        public void UpdateAndroidVersion(string version)
        {
            AndroidVersion = version;
        }

        public void UpdateModel(string model)
        {
            Model = model;
        }
    }
}
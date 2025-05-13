using Android.Content;
using Android.OS;
using swissbyte.Platforms.Android;
using swissbyte.Interfaces;
using Application = Android.App.Application;

[assembly: Dependency(typeof(BatteryService))]

namespace swissbyte.Platforms.Android
{
    public class BatteryService : IBatteryService
    {
        public string GetBatteryHealth()
        {
            using var filter = new IntentFilter(Intent.ActionBatteryChanged);
            using var battery = Application.Context.RegisterReceiver(null, filter);

            int health = battery?.GetIntExtra(BatteryManager.ExtraHealth, -1) ?? -1;

            return health switch
            {
                (int)BatteryHealth.Cold => "Fría",
                (int)BatteryHealth.Dead => "Muerta",
                (int)BatteryHealth.Good => "Buena",
                (int)BatteryHealth.Overheat => "Sobrecalentada",
                (int)BatteryHealth.OverVoltage => "Sobretensión",
                (int)BatteryHealth.Unknown => "Desconocido",
                (int)BatteryHealth.UnspecifiedFailure => "Fallo no especificado",
                _ => "No disponible"
            };
        }

        public string GetBatteryType()
        {
            using var filter = new IntentFilter(Intent.ActionBatteryChanged);
            using var battery = Application.Context.RegisterReceiver(null, filter);
            var technology = battery?.GetStringExtra(BatteryManager.ExtraTechnology);

            return technology ?? "Desconocido";
        }

        public string GetTemperature()
        {
            using var filter = new IntentFilter(Intent.ActionBatteryChanged);
            using var battery = Application.Context.RegisterReceiver(null, filter);
            int temp = battery?.GetIntExtra(BatteryManager.ExtraTemperature, -1) ?? -1;

            return temp >= 0 ? $"{temp / 10f:0.0} °C" : "Desconocida";
        }

        public string GetVoltage()
        {
            using var filter = new IntentFilter(Intent.ActionBatteryChanged);
            using var battery = Application.Context.RegisterReceiver(null, filter);
            int voltage = battery?.GetIntExtra(BatteryManager.ExtraVoltage, -1) ?? -1;

            return voltage >= 0 ? $"{voltage} mV" : "Desconocido";
        }
    }
}

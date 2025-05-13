using Microsoft.Extensions.Logging;
using swissbyte.Interfaces;
#if ANDROID
using swissbyte.Platforms.Android;
#endif

namespace swissbyte
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #if DEBUG
    		builder.Logging.AddDebug();
            #endif

            #if ANDROID
            builder.Services.AddSingleton<IBatteryService, BatteryService>();
            builder.Services.AddSingleton<IDeviceInfoService, DeviceInfoService>();
            #endif
            
            return builder.Build();
        }
    }
}

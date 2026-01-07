using Camera.MAUI;
using Microsoft.Extensions.Logging;
using ProyectoInterfazNatural.Services;
using ProyectoInterfazNatural.ViewModels;
using ProyectoInterfazNatural.Views;

namespace ProyectoInterfazNatural
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
                })
                .UseMauiCameraView();
              
            //vamos a registrar el servicio para el id del dispositivo
#if ANDROID
            builder.Services.AddSingleton<IDeviceService, ProyectoInterfazNatural.Platforms.DeviceService>();
            builder.Services.AddSingleton<IVozAndroidService, ProyectoInterfazNatural.Platforms.Android.VozAndroidService>();
#endif
           
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

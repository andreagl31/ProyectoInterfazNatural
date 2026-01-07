#if ANDROID
using ProyectoInterfazNatural.Services;
using Android.Provider;
using Android.Content;

namespace ProyectoInterfazNatural.Platforms
{
    public class DeviceService : IDeviceService
    {
        public string GetDeviceId()
        {
            Context? context = global::Android.App.Application.Context;
            string? deviceId = Settings.Secure.GetString(context?.ContentResolver, Settings.Secure.AndroidId);
            return deviceId ?? string.Empty;
        }
        //este servicio sirve para que te devuelva el id del dispositivo actual
    }
}
#endif
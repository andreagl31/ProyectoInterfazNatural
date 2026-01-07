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
            //Gracias al objeto Settings.Secure podemos obtener el ID del dispositivo y retornarlo para posteriormente 
            //usarlo en la lista de dispositivos usados
            string? deviceId = Settings.Secure.GetString(context?.ContentResolver, Settings.Secure.AndroidId);
            return deviceId ?? string.Empty;
        }
        //este servicio sirve para que te devuelva el id del dispositivo actual
    }
}
#endif
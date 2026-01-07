using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Speech;
using Plugin.Fingerprint;
using ProyectoInterfazNatural.Services;

namespace ProyectoInterfazNatural
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // CRÍTICO: Configurar resolver de Fingerprint
            CrossFingerprint.SetCurrentActivityResolver(() => this);
        }

        protected override void OnActivityResult(int requestCode, [Android.Runtime.GeneratedEnum] Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1001)
            {
                // Obtenemos el servicio gracias a platforms android
                var vozService = IPlatformApplication.Current?.Services.GetService<IVozAndroidService>() as Platforms.Android.VozAndroidService;
                // Si el resultado es OK y data no es nulo
                if (resultCode == Result.Ok && data != null)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults); // Obtenemos los resultados del reconocimiento de voz
                    string texto = matches?.FirstOrDefault() ?? string.Empty; //Tomamos el primer string 
                    vozService?.Completar(texto); //usamos el método que definimos en el servicio para completar el texto
                }
                else
                {
                    vozService?.Completar(string.Empty);
                }
            }
        }
    }
}
/*En resumen para que se entienda mejor el flujo:
 * 1-El viewmodel espera gracias a await a la promesa que guarda el resultado del reconocimiento devoz
 * 2- De mientras en vozAndroidService se crea la promesa que guardará el resultado del reconomiento de voz
 * 3- El MainActivity usa el servicio de voz para completar la promesa y meter el resultado del reconocimiento de voz
 * Recuerda que el mainactivity es lo que se ejecutará primero al inciar la app en un dispositivo android por lo tanto el flujo real es:
 * MainActivity -> VozAndroidService -> ViewModel (que espera)
*/

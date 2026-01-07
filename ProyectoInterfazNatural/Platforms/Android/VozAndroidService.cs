//Esta clase es necesaria para el reconocimiento de voz nativo en Android es decir vamos a usar la API de google
//en android para reconocer la voz.
using Android.Content;
using Android.Speech;
using ProyectoInterfazNatural.Services;
using System;
using System.Threading.Tasks;
//Esta clase esta en platforms android ya que solo funciona en android y usa clases de android como intent y recognizerintent
namespace ProyectoInterfazNatural.Platforms.Android
{ 
  // Implementa la interfaz IVozAndroidService que es una interfaz que está en la carpeta de todo el proyecto 
  //para evitarnos errores de que el viewmodel dependa solo de Android y además para poder usarlo en cualquier parte del proyecto
    public class VozAndroidService : IVozAndroidService
    {
        //Este tcs es un objeto que permite manejar un resultado asincrono (como una promesa en javascript)
        private TaskCompletionSource<string> _tcs;
        //Este método es asíncrono ya que el usuario puede tardar en hablar y android responder más tarde.
        public Task<string> IniciarReconocimientoAsync()
        {
            _tcs = new TaskCompletionSource<string>();
            // Este intent le dice a Android que quiere usar el reconocimiento de voz (ActionRecognizeSpeech)
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            //Sabemos que los intent le podemos meter parámetros antes de iniciarlo así que le podemos el párametro de usar voz libre.
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            //Le ponemos el parámetro de que muestre el mensaje siguiente
            intent.PutExtra(RecognizerIntent.ExtraPrompt, "Di el nombre del libro");
            // Le decimos que use el idioma por defecto del dispositivo
            intent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            // Obtenemos la activity actual
            var activity = Platform.CurrentActivity;
            // Iniciamos la activity está encima de la actual gracias a nuestro intent, le añadimos un código identificador
            activity.StartActivityForResult(intent, 1001);

            return _tcs.Task; // Te devuelve la promesa cumplida una vez que hemos ejecutado el reconocimiento de voz
        }
        //Completamos la tarea pendiente que es pedir la voz, este método se llama desde el MainActivity una vez que se ha obtenido el resultado del reconocimiento de voz
        public void Completar(string texto)
        {
            _tcs?.SetResult(texto);
        }
    }
}

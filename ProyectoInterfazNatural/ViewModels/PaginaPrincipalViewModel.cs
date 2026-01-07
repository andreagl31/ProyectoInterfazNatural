using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;

namespace ProyectoInterfazNatural.ViewModels;

public class PaginaPrincipalViewModel
{
    public User UsuarioActualApp { get; set; }
    // Como los libros lo necesitamos sobretodo en la pantalla de buscar, lo más lógico es que residan en dicho viewmodel
    //Pero aquí los necesitaremos simplemente para buscar el libro según el nombre por ello hago una instancia momentanea de dichos
    //libros, recordemos que la lista de libros es como una base de datos que no puede tocar el usuario
    public ObservableCollection<Book> AllBooks
    {
        get
        {
            return new BuscarViewModel(UsuarioActualApp).AllBooks;
        }
    }
    public ICommand BuscarLibroCommand { get; }
    public string Resultado { get; private set; } = string.Empty;
    public INavigation Navigation { get; set; } //nos da un controlador para manejar la navegación desde aquí
    private readonly IVozAndroidService? _vozService; //el servicio que haremos para que funcione el reconocimiento de voz

    public PaginaPrincipalViewModel(User user, INavigation navigation)
    {
        UsuarioActualApp = user;
        Navigation = navigation;

        // Obtenemos el servicio de voz gracias a nuestro objeto Services que guarda todos los servicios registrados
        _vozService = IPlatformApplication.Current?.Services.GetService<IVozAndroidService>(); 

        BuscarLibroCommand = new Command(async () => await EjecutarReconocimientoVoz());
    }
    // La función que se encarga de ejecutar el reconocimiento de voz
    private async Task EjecutarReconocimientoVoz()
    {
        try
        {
            //Usamos el TextToSpeech para pedir al usuario que diga el nombre del libro
            await TextToSpeech.SpeakAsync("Di el nombre del libro");

            // Pedimos permiso de micrófono, muy importante primero
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted) //Si el usuario no nos da el permiso, saldremos del método sin hacer nada
            {
                var page = Application.Current?.Windows?.FirstOrDefault()?.Page;
                if (page != null)
                    await page.DisplayAlert("Permiso", "Se necesita el micrófono", "OK");
                return;
            }

            // Verificar que el servicio está disponible
            // Aplication current es la aplicación actual, windows es la colección de ventanas abiertas y first es la que 
            //está primera en la pila es decir la que estamos viendo, esto sirve para mostrar alertas en la pantalla actual
            if (_vozService == null)
            {
                var page = Application.Current?.Windows?.FirstOrDefault()?.Page;
                if (page != null)
                    await page.DisplayAlert("Error", "No se pudo acceder al servicio de voz", "OK");
                return;
            }
            //Guardamos en resultado lo que nos devuelve el servicio de voz
            //Como vemos estamos usando el servicio que hemos definido
            string resultado = await _vozService.IniciarReconocimientoAsync();
            //Si no se reconoce nada , mostramos un mensaje de error
            if (string.IsNullOrWhiteSpace(resultado))
            {
                var page = Application.Current?.Windows?.FirstOrDefault()?.Page;
                if (page != null)
                    await page.DisplayAlert("No reconocido", "No se detectó voz", "OK");
                return;
            }
            // Lo pasamos a minúsculas para facilitar la búsqueda
            Resultado = resultado.ToLowerInvariant();

            // Buscamos el libro por título, le dice de todos los libros coge el primero
            //que contenga en su título el texto reconocido y pasalo a minúsculas para evitar problemas
            var libro = AllBooks
                .FirstOrDefault(b => b.Title?.ToLowerInvariant().Contains(Resultado) == true);
            //Si encontramos el libro, usamos de nuevo TextToSpeech para decirle al usuario que hemos encontrado el libro
            if (libro != null)
            {
                await TextToSpeech.SpeakAsync($"Libro encontrado: {libro.Title}");               
                    await Navigation.PushAsync(new ProyectoInterfazNatural.Views.PerfilLibroView(UsuarioActualApp, libro));
            }
            else //Si no encontramos el libro, le decimos que no se ha encontrado
            {
                await TextToSpeech.SpeakAsync("No se encontró el libro con ese nombre");
            }
        } //Si hay cualquier excepción la manejaremos aquí diciendole al usuario que ocurrió un error
        catch (Exception ex)
        {
            await TextToSpeech.SpeakAsync("Ocurrió un error al reconocer la voz");
            Debug.WriteLine($"Error Reconocimiento de voz: {ex.Message}");
        }
    }
}

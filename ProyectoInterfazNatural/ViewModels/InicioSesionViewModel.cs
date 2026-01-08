using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using ProyectoInterfazNatural.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//iniciosesion
namespace ProyectoInterfazNatural.ViewModels
{
    public class InicioSesionViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        private readonly IDeviceService _deviceService;
        private bool _isProcessing;
        public ICommand LoginWithBiometricCommand { get; }
        public ICommand LoginWithPasswordCommand { get; }

        public InicioSesionViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            Users = new ObservableCollection<User>();
            LoginWithBiometricCommand = new Command(async () => await ExecuteLoginWithBiometric(), () => !_isProcessing);
            LoginWithPasswordCommand = new Command(async () => await ExecuteLoginWithPassword(), () => !_isProcessing);
        }
        //wrapper para ver si estamos procesando ya alguna acción de login
        private async Task ExecuteLoginWithBiometric()
        {
            if (_isProcessing) return;
            _isProcessing = true;
            ((Command)LoginWithBiometricCommand).ChangeCanExecute();
            
            var user = await LoginWithBiometric();
            
            // Si el login es exitoso, navegar a la página principal
            if (user != null)
            {
                Application.Current.MainPage = new NavigationPage(new PaginaPrincipalView(user))
                {
                    BarBackgroundColor = Colors.Transparent
                };
            }
            
            _isProcessing = false;
            ((Command)LoginWithBiometricCommand).ChangeCanExecute();
        }

        private async Task ExecuteLoginWithPassword()
        {
            if (_isProcessing) return;
            _isProcessing = true;
            ((Command)LoginWithPasswordCommand).ChangeCanExecute();
            
            var user = await LoginWithPassword();
            
            // Si el login es exitoso, navegar a la página principal
            //De normal nosotros siempre hemos navegado desde nuestro cs del view
            //Pero como queremos que sea una lógica más compacta, al tener dos opciones de navegación
            //es decir o bien con contraseña o bien con huella, es mejor controlarlo con commands desde el view
            //ya que si no tendrías que poner command(para manejar la lógica de usuarios) y clicked ( para navegar) simultaneamente lo que no es muy correcto
            if (user != null)
            {
                Application.Current.MainPage = new NavigationPage(new PaginaPrincipalView(user))
                {
                    BarBackgroundColor = Colors.Transparent
                };
            }
            
            _isProcessing = false;
            ((Command)LoginWithPasswordCommand).ChangeCanExecute();
        }

        //task pk es una función asincrona, tiene que ser asicrona por el uso de huella
        public async Task<User> LoginWithBiometric()
        {
            //obtenemos el usuario por el nombre
            var user = Users.FirstOrDefault(user => user.Username == Username);
            if (user == null)
            {
                //mostrar alerta personalizada
                await Application.Current.MainPage.DisplayAlert(
                  "Usuario no encontrado",
                  "Este usuario no existe, por favor regístrate",
                  "OK"
              );
                return null;
            }
            //si el usuario no tiene activada la huella
            if (!user.IsBiometricEnabled)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Huella no disponible",
                    "Este usuario no tiene activado el inicio con huella. Usa contraseña.",
                    "OK");
                return null;
                
            }

            // obtenemos el id del dispositivo en concreto gracias al servicio
            string deviceId = _deviceService.GetDeviceId();

            //Si no esta en este dispositivo
            if (!user.DevicesWithBiometrics.Contains(deviceId))
            {
                await Application.Current.MainPage.DisplayAlert(
                "Dispositivo no autorizado",
                "Debes iniciar sesión con contraseña en este dispositivo primero.",
                "OK");
                return null; 
            }
                //si esta en el dispositivo pasamos a pedir la huella
                var result = await CrossFingerprint.Current.AuthenticateAsync(
                        new AuthenticationRequestConfiguration("Inicio de sesión", "Coloca tu dedo")

                    );
                if (!result.Authenticated) //si la huella no esta en el dispositivo te dará error
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Huella incorrecta", "OK");
                    return null;
                }
            //Login exitoso
            return user;

            }
     
        //retorna un el usuario para así poder cogerlo en la vista y pasarlo a la siguiente página
        public async Task<User> LoginWithPassword() { 
            var user= Users.FirstOrDefault(user => user.Username == Username);
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Usuario no encontrado",
                    "Este usuario no existe,o la contraseña es incorrecta por favor regístrate",
                    "OK"
                );
                return null;
            }
            //ahora verificamos contraseña
            if (Password != user.Password)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Contraseña incorrecta", "OK");
                return null;
            }

            // ahora una vez que iniciamos sesión preguntaremos si quiera activar la biometria si no esta activada
            string deviceId = _deviceService.GetDeviceId();
            if (!user.IsBiometricEnabled)
            {
                bool activarBiometria = await Application.Current.MainPage.DisplayAlert(
                    "Huella", "¿Deseas activar inicio con huella en este dispositivo?", "Sí", "No"
                    );

                if (activarBiometria)
                {
                    user.IsBiometricEnabled = true;
                    // Solo añadir el dispositivo si no está ya en la lista
                    if (!user.DevicesWithBiometrics.Contains(deviceId))
                    {
                        user.DevicesWithBiometrics.Add(deviceId);
                    }
                }
            }
            else
            {
                // Si la biometría ya está activada pero este dispositivo no está registrado
                if (!user.DevicesWithBiometrics.Contains(deviceId))
                {
                    bool activarEnEsteDispositivo = await Application.Current.MainPage.DisplayAlert(
                        "Huella", 
                        "¿Deseas activar inicio con huella en este dispositivo?", 
                        "Sí", 
                        "No"
                    );

                    if (activarEnEsteDispositivo)
                    {
                        user.DevicesWithBiometrics.Add(deviceId);
                    }
                }
            }

            //Login exitoso
            return user;
        }
    }
}





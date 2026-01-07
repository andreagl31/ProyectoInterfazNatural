using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoInterfazNatural.ViewModels
{
    public class RegistroViewModel
    {
        private ObservableCollection<User> users;
        private readonly IDeviceService _deviceService;
        private bool _isProcessing;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseBiometric { get; set; }
        public ICommand RegistrarCommand { get; }

        public RegistroViewModel(InicioSesionViewModel vm, IDeviceService deviceService)
        {
            users = vm.Users;
            _deviceService = deviceService;
            //UseBiometric va a guardar un booleano si el usuario se quiere identificar con huella, o bien entrar con contraseña
            //La huella vive en el sistema operativo, así que antes tiene que estar registrada en el sistema operativo para poder
            //usarse, la huella determina el dueño del teléfono, pero la primera vez que te registres es cuando tienes que definir la contraseña
            RegistrarCommand = new Command(async () => await ExecuteRegistrar(), () => !_isProcessing);
        }

        private async Task ExecuteRegistrar()
        {
            if (_isProcessing) return;
            _isProcessing = true;
            ((Command)RegistrarCommand).ChangeCanExecute();

            //Antes que nada tenemos que verificar si el usuario ya existe:
            var usuarioExistente = users
            .FirstOrDefault(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase));

            if (usuarioExistente != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Usuario existente",
                    "Ya existe un usuario con ese nombre",
                    "OK"
                );
                _isProcessing = false;
                ((Command)RegistrarCommand).ChangeCanExecute();
                return;
            }

            string deviceId = null;
            if (UseBiometric)
            {

                deviceId = _deviceService.GetDeviceId();
            }
            var nuevoUsuario = new User
            //este constructor usa los setters
            {
                Username = Username,
                Password = Password,
                IsBiometricEnabled = UseBiometric,
                myBooks = new List<Book>(),
                DevicesWithBiometrics = UseBiometric && deviceId != null
                ? new List<string> { deviceId }
                : new List<string>()
                //si esta activado guardo en los dispodsitivos este dispositivo
            };
            users.Add(nuevoUsuario);
            //para mostrar un mensaje
            await Application.Current.MainPage.DisplayAlert("Registro exitoso",
                                                            $"Usuario {Username} creado",
                                                            "OK");
            // Limpiamos campos
            Username = string.Empty;
            Password = string.Empty;
            UseBiometric = false;

            // Volver automáticamente al inicio de sesión después del registro exitoso, para evitar sobrepuestos
            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage?.Navigation?.NavigationStack?.Count > 1)
            {
                await navigationPage.Navigation.PopAsync();
            }

            _isProcessing = false;
            ((Command)RegistrarCommand).ChangeCanExecute();
        }
    }
}
    //Por seguridad vamos a hacer que si es la primera vez de el inicio de sesión de un usuario en un dispositivo
    //tenga que poner la contraseña, cuando nos aseguramos que el dueño del dispositivo sabe sobre la cuenta
    //ya es cuando te deja poner la huella para iniciar sesión
    //Si no hacemos esto podriamos iniciar sesión en el dispositivo con nuestra huella en una cuenta
    // de una persona que tenga la huella activada


using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoInterfazNatural.ViewModels
{
    public class InicioSesionViewModel

    {
        public ObservableCollection<User> Users { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        private readonly IDeviceService _deviceService;
        public ICommand LoginWithBiometricCommand { get; }
        public ICommand LoginWithPasswordCommand { get; }

        public InicioSesionViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            Users = new ObservableCollection<User>();
            LoginWithBiometricCommand = new Command(async () => await LoginWithBiometric());
            LoginWithPasswordCommand = new Command(async () => await LoginWithPassword());
       
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
                    "Este usuario no existe, por favor regístrate",
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
            if (!user.IsBiometricEnabled || !user.DevicesWithBiometrics.Contains(deviceId))
            {
                bool activarBiometria = await Application.Current.MainPage.DisplayAlert(
                    "Huella", "¿Deseas activar inicio con huella en este dispositivo?", "Sí", "No"
                    );

                if (activarBiometria)
                {
                    user.IsBiometricEnabled = true;
                    user.DevicesWithBiometrics.Add(deviceId);
                }//añadimos el dispositivo a la lista de dispositios en donde hemos iniciado sesion y ponemos la biometria como que si
            }

            //Login exitoso
            return user;
        }
    }
}





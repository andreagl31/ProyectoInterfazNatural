using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInterfazNatural.Model
{
    [AddINotifyPropertyChangedInterface]
    public class User
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public bool IsBiometricEnabled {  get; set; }
        public List <Book> myBooks { get; set; }
        // Vmos a añadir la lista de dispositivos deonde el usuario activo la biometría, es decir la lista de dispositivos
        //doinde se registro
        public List<string> DevicesWithBiometrics { get; set; } = new List<string>();
      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInterfazNatural.Services
{
    public interface IVozAndroidService
    {
        Task<string> IniciarReconocimientoAsync();
    }
}

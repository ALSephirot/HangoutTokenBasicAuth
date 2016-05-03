using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAutorizador.Models
{
    public class UserTemp
    {
        public UserTemp() { }

        public UserTemp(string _NombreUsuario, string _Contraseña)
        {
            NombreUsuario = _NombreUsuario;
            Contraseña = _Contraseña;
        }

        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }
}

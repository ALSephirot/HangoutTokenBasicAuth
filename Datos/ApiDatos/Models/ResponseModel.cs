using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDatos.Models
{
    public class ResponseModel<T>
    {
        public bool Autorizado { get; set; }
        public bool Expired { get; set; }
        public string Token { get; set; }
        public string Mensaje { get; set; }
        public T ObjetoRespuesta { get; set; }
    }
}

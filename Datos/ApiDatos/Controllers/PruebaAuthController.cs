using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiDatos.Controllers
{
    public class PruebaAuthController : ApiController
    {
        [HttpGet]
        [Route("api/PruebaAuth/Autorizado")]
        [Authorize]
        public string MetodoAuthenticado()
        {
            return "Si ves esto es porque estas autorizado";
        }

        [HttpGet]
        [Route("api/PruebaAuth/NoAutorizado")]
        public string MetodoNoAuthenticado()
        {
            return "Este metodo no necesita autorizacion";
        }
    }
}

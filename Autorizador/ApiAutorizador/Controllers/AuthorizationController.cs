using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiAutorizador.Models;

namespace ApiAutorizador.Controllers
{
    public class AuthorizationController : ApiController
    {
        Utilities.Authorization Auth = new Utilities.Authorization();

        [HttpPost]
        public IHttpActionResult Authorization(UserTemp Usuario)
        {
            var Token = string.Empty;
            var Respuesta = new ResponseModel();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Token = Auth.Autorizacion(Usuario);
                Respuesta.Token = Token;
                Respuesta.Autorizado = true;
                Respuesta.Mensaje = "Autorizado Correctamente";
            }
            catch(Exception ex)
            {
                Respuesta.Token = "";
                Respuesta.Autorizado = false;
                Respuesta.Mensaje = ex.Message;
            }

            return Json<ResponseModel>(Respuesta);
        }

        [HttpGet]
        public IHttpActionResult VerifyAuthorization(string Token)
        {
            var Respuesta = new ResponseModel<Tb_Usuarios>();

            if(string.IsNullOrEmpty(Token))
            {
                Respuesta.Token = Token;
                Respuesta.Autorizado = false;
                Respuesta.Mensaje = "El token no puede llegar vacio";

                return Ok(Respuesta);
            }

            try
            {
                var usuario = Auth.VerificarAutorizacion(Token);
                Respuesta.Token = Token;
                Respuesta.Mensaje = "Token Válido";
                Respuesta.Autorizado = true;
                Respuesta.Expired = false;//Un metodo que valide la vigencia del token enviado.
                Respuesta.ObjetoRespuesta = usuario;
            }
            catch(Exception ex)
            {
                Respuesta.Token = Token;
                Respuesta.Autorizado = false;
                Respuesta.Mensaje = ex.Message;
            }

            return Json<ResponseModel<Tb_Usuarios>>(Respuesta);
        }

        [HttpGet]
        [Route("api/Authorization/Refresh")]
        public IHttpActionResult RefreshAuthorization(string Token)
        {
            var Respuesta = new ResponseModel();

            if (string.IsNullOrEmpty(Token))
            {
                Respuesta.Token = Token;
                Respuesta.Autorizado = false;
                Respuesta.Mensaje = "El token no puede llegar vacio";

                return Ok(Respuesta);
            }

            try
            {
                var TokenNew = Auth.RefreshToken(Token);
                Respuesta.Token = TokenNew;
                Respuesta.Mensaje = "Token Válido. Token reasignado correctamente";
                Respuesta.Autorizado = true;
            }
            catch(Exception ex)
            {
                Respuesta.Token = Token;
                Respuesta.Autorizado = false;
                Respuesta.Mensaje = ex.Message;
            }

            return Json<ResponseModel>(Respuesta);
        }

        public class ResponseModel
        {
            public bool Autorizado { get; set; }
            public string Token { get; set; }
            public string Mensaje { get; set; }
        }

        public class ResponseModel<T>
        {
            public bool Autorizado { get; set; }
            public bool Expired { get; set; }
            public string Token { get; set; }
            public string Mensaje { get; set; }
            public T ObjetoRespuesta { get; set; }
        }
    }
}

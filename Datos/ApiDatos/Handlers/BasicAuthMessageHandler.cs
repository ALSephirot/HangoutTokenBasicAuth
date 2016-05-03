using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiDatos.Models;
using ApiDatos.Utilities;
using System.Threading;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiDatos.Handlers
{
    class BasicAuthMessageHandler:DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = request.Headers;

            if(headers.Authorization != null && headers.Authorization.Scheme == "Basic")
            {
                var cw = new ConsumirWebApi();
                var result = cw.Peticion("Authorization", headers.Authorization.Parameter);
                var Respuesta = JsonConvert.DeserializeObject<ResponseModel<Tb_Usuarios>>(result);

                if(Respuesta.Autorizado)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,Respuesta.ObjetoRespuesta.id.ToString()),
                        new Claim(ClaimTypes.Name, Respuesta.ObjetoRespuesta.NombreUsuario),
                        new Claim(ClaimTypes.Role, Respuesta.ObjetoRespuesta.FK_Rol.ToString())
                    }, "ApplicationCookie");

                    var ClaimsPpal = new ClaimsPrincipal(identity);

                    PutPrincipal(ClaimsPpal);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }

        private void PutPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if(HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}

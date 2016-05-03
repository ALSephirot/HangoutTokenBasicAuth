using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAutorizador.Models;
using System.Collections.Concurrent;

namespace ApiAutorizador.Utilities
{
    class Authorization
    {
        public static ConcurrentDictionary<Guid, AuthorizationModel> Autorizados = new ConcurrentDictionary<Guid, AuthorizationModel>();
        Encoder Encode = new Encoder();
        Ink_line_DBEntities db = new Ink_line_DBEntities();

        public string Autorizacion(UserTemp usuario)
        {
            var Token = string.Empty;

            try
            {
                /*
                 * El proyecto no va a correr ya que he quitado la autenticacion de la base de datos.
                 * Simplemente creen el una base de datos igual y cambien las connection Strings para que les funcione.
                 * o vuelvan a generar el modelo
                 */
                var DBUsuario = db.Tb_Usuarios.Where(mm => mm.NombreUsuario == usuario.NombreUsuario).FirstOrDefault();

                if(DBUsuario != null)
                {
                    if (DBUsuario.Contrasena == usuario.Contraseña)
                    {
                        var id = Guid.NewGuid();

                        var Authorization = new AuthorizationModel() { FechaCreacion = DateTime.Now, NombreUsuario = DBUsuario.NombreUsuario, TiempoSession = 30 };

                        Autorizados.TryAdd(id, Authorization);

                        Token = Encode.Base64Encode(Convert.ToString(id));
                    }
                    else
                    {
                        throw new Exception("La contraseña es incorrecta");
                    }
                }
                else
                {
                    throw new Exception("El usuario es incorrecto");
                }
            }
            catch
            {
                throw;
            }

            return Token;
        }

        public Tb_Usuarios VerificarAutorizacion(string Token)
        {
            var _id = Encode.Base64Decode(Token);
            var id = Guid.Parse(_id);
            var usuario = new Tb_Usuarios();
            var Auth = new AuthorizationModel();

            try
            {
                if(Autorizados.TryGetValue(id, out Auth))
                {
                    usuario = db.Tb_Usuarios.Where(mm => mm.NombreUsuario == Auth.NombreUsuario).FirstOrDefault();
                }
                else
                {
                    throw new Exception("Token Inválido");
                }
            }
            catch
            {
                throw;
            }

            return usuario;
        }

        public string RefreshToken(string TokenAnt)
        {
            var _id = Encode.Base64Decode(TokenAnt);
            var id = Guid.Parse(_id);
            var Auth = new AuthorizationModel();
            var Token = string.Empty;

            try
            {
                if(Autorizados.TryRemove(id, out Auth))
                {
                    var idNuevo = Guid.NewGuid();

                    var Authorization = new AuthorizationModel() { FechaCreacion = DateTime.Now, NombreUsuario = Auth.NombreUsuario, TiempoSession = 30 };

                    Autorizados.TryAdd(idNuevo, Authorization);

                    Token = Encode.Base64Encode(Convert.ToString(idNuevo));
                }
                else
                {
                    throw new Exception("Token Inválido");
                }
            }
            catch
            {
                throw;
            }

            return Token;
        }
    }

    class AuthorizationModel
    {
        public DateTime FechaCreacion { get; set; }
        public string NombreUsuario { get; set; }
        public int TiempoSession { get; set; }
    }
}

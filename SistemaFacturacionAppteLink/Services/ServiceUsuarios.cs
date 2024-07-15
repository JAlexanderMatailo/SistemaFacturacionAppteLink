using Microsoft.AspNetCore.Mvc;
using SisCazhapugroBack.Utilidades;
using SistemaFacturacionAppteLink.Infraestructura;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Services
{
    public class ServiceUsuarios : IUsuario
    {
        MensajesVM mensajeria = new MensajesVM();
        SistemaFacturacionContext _context = new SistemaFacturacionContext();

        public ServiceUsuarios(SistemaFacturacionContext context)
        {
            _context = context;
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }
            // Convertir a minúsculas y eliminar espacios   
            return string.Concat(texto.ToLower().Where(c => !char.IsWhiteSpace(c)));
        }
        private bool ProductoExistePorNombre(string nombreUsuario)
        {
            string descripcionNormalizada = NormalizarTexto(nombreUsuario);

            var descripcionesExistentes = _context.Usuarios.ToList()
                .Select(x => new { x.Usuario1 })
                .AsEnumerable() // Cambia a ejecución en memoria
                .ToList();
            var existe = descripcionesExistentes.Any(x => NormalizarTexto(x.Usuario1) == descripcionNormalizada);
            return existe;
        }
        public JsonResult SetUsuarios(UsuarioVMRequest usuario)
        {
            //var existeUser = _context.Usuarios.Where(x => x.Usuario1 == usuario.Usuario1).Any();
            var existeUserCorreo = _context.Usuarios.Where(x => x.Correo == usuario.Correo).Any();
            try
            {
                if (!ProductoExistePorNombre(usuario.Usuario1))
                {
                    if (!existeUserCorreo)
                    {
                        using (var context = _context.Database.BeginTransaction())
                        {
                            try
                            {

                                Usuario user = new Usuario()
                                {
                                    Usuario1 = usuario.Usuario1,
                                    Correo = usuario.Correo,
                                    Contrasena = usuario.Contrasena
                                };
                                _context.Usuarios.Add(user);
                                _context.SaveChanges();
                                context.Commit();
                                mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                                mensajeria.mensajeDescripcion = MensajeExito.MensajeRegistro;

                            }
                            catch (Exception ex)
                            {
                                mensajeria.codigoResult = (int)Codigos.CodigoError;
                                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeError + ex.Message;
                                context.Rollback();
                            }
                        }
                    }
                    else
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoFail;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeExisteUsuarioCorreo;
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoFail;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeExisteUsuario;
                }

            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeNoConexion + ex.Message;
            }
            return new JsonResult(mensajeria);
        }
        public ResultUsersLogin DatoUsuario(string nombreUsuario, string password)
        {
            ResultUsersLogin result = new ResultUsersLogin()
            {
                Usuario = new UsuariosVMResponse()
            };
            var usuario = _context.Usuarios.Where(x => x.Usuario1 == nombreUsuario).Any();
            var contraseña = _context.Usuarios.Where(x => x.Contrasena == password).Any();
            if (usuario)
            {
                if (contraseña)
                {
                    var content = _context.Usuarios.Where(x => x.Usuario1 == nombreUsuario && x.Contrasena == password).FirstOrDefault();
                    if (content != null)
                    {
                        result.Usuario.IdUsuario = content.IdUsuario;
                        result.Usuario.nombreUsuario = content.Usuario1;
                        result.Usuario.token = Token.GenerarToken(content.Usuario1);
                        result.Usuario.Correo = content.Correo;

                        result.codigoResult = (int)Codigos.CodigoSuccess;
                        result.mensajeDescripcion = MensajeExito.MensajeAutorizacion;
                        //return content;
                    }
                }
                else
                {
                    var intentos = IntentosFallidos(nombreUsuario);
                    result.codigoResult = intentos.codigoResult;
                    result.mensajeDescripcion = intentos.mensajeDescripcion;
                    //result.codigoResult = (int)Codigos.CodigoFail;
                    //result.mensajeDescripcion = MensajeExcepciones.MensajeIPassword;
                }
            }
            else
            {
                result.codigoResult = (int)Codigos.CodigoFail;
                result.mensajeDescripcion = MensajeExcepciones.MensajeIUser;
            }
            return result;
        }
        /*
                public JsonResult IntentosFallidos(string nombreUsuario, string password)
                {
                    var usuario = _context.Usuarios.Where(x => x.Usuario1 == nombreUsuario && x.Bloqueado == false).Any();
                    var contraseña = _context.Usuarios.Where(x => x.Contrasena == password).Any();
                    try
                    {
                        if (usuario && !contraseña)
                        {
                            var content = _context.Usuarios.Where(x => x.Usuario1 == nombreUsuario && x.IntentosFallidos == 0).FirstOrDefault();
                            if (content != null && content.IntentosFallidos <= 3)
                            {
                                using (var context = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        content.IntentosFallidos = +1;

                                        _context.SaveChanges();
                                        context.Commit();

                                        mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeIPassword;
                                    }
                                    catch (Exception ex)
                                    {
                                        mensajeria.codigoResult = (int)Codigos.CodigoFail;
                                        mensajeria.mensajeDescripcion = "Error al conectar al servicio de bloqueo de usuarios" + ex.Message;
                                    }

                                }
                            }
                            else
                            {
                                mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeBUser;
                            }

                        }
                        else
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeBUser;
                        }
                    }
                    catch (Exception ex)
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoError;
                        mensajeria.mensajeDescripcion = "Error al consultar usuarios: " + ex.Message;
                    }

                    return new JsonResult(mensajeria);
                }*/
        public (int codigoResult, string mensajeDescripcion) IntentosFallidos(string nombreUsuario)
        {
            var usuario = _context.Usuarios.Any(x => x.Usuario1 == nombreUsuario && x.Bloqueado == false);
            var contraseña = _context.Usuarios.Any(x => x.Contrasena == nombreUsuario);

            try
            {
                if (usuario && !contraseña)
                {
                    var content = _context.Usuarios.FirstOrDefault(x => x.Usuario1 == nombreUsuario && x.IntentosFallidos <= 4);
                    if (content != null && content.IntentosFallidos <= 3)
                    {
                        using (var context = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                content.IntentosFallidos += 1;

                                _context.SaveChanges();
                                context.Commit();

                                mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeIPassword;
                            }
                            catch (Exception ex)
                            {
                                mensajeria.codigoResult = (int)Codigos.CodigoFail;
                                mensajeria.mensajeDescripcion = "Error al conectar al servicio de bloqueo de usuarios: " + ex.Message;
                            }
                        }
                    }
                    else
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeBUser;
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoFail;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeBUser;
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = "Error al consultar usuarios: " + ex.Message;
            }

            return (mensajeria.codigoResult, mensajeria.mensajeDescripcion);
        }
        public ResultUsers GetUsersHist()
        {
            ResultUsers result = new ResultUsers
            {
                ListUsuarios = new List<UsuarioVMResponse>()
            };
            try
            {
                var Usiarios = _context.Usuarios.ToList();
                foreach (var usuario in Usiarios)
                {
                    UsuarioVMResponse user = new UsuarioVMResponse
                    {
                        IdUsuario = usuario.IdUsuario,
                        Usuario1 = usuario.Usuario1,
                        Contrasena = usuario.Contrasena,
                        Correo = usuario.Correo,
                        IntentosFallidos = usuario.IntentosFallidos,
                        Bloqueado = usuario.Bloqueado
                    };
                    result.ListUsuarios.Add(user);
                }
                result.codigoResult = (int)Codigos.CodigoSuccess;
                result.mensajeDescripcion = MensajeExito.MensajeSucces;

            }
            catch (Exception ex)
            {
                result.codigoResult = (int)Codigos.CodigoError;
                result.mensajeDescripcion = "Error al consultar usuarios: " + ex.Message;
            }

            return result;
        }

        public JsonResult UpdateUserPassword(UsuarioVMRequest usuarios)
        {
            try
            {
                var user = _context.Usuarios.FirstOrDefault(x => x.Usuario1 == usuarios.Usuario1);
                if (user != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            user.Contrasena = usuarios.Contrasena;

                            _context.SaveChanges();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeUpdate;
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = "Error al conectar al servicio para la actualizacion de datos" + ex.Message;
                        }
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoFail;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeUpdateE;
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = "Error al actualizar el usuario: " + ex.Message;
            }

            return new JsonResult(mensajeria);
        }

        public JsonResult UpdateUser(int IdUsuario, UsuarioVMRequest usuarios)
        {
            try
            {
                var user = _context.Usuarios.FirstOrDefault(x => x.IdUsuario == IdUsuario);
                if (user != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            user.Usuario1 = usuarios.Usuario1;
                            user.Correo = usuarios.Correo;
                            user.Contrasena = usuarios.Contrasena;

                            _context.SaveChanges();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeUpdate;
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = "Error al conectar al servicio para la actualizacion de datos" + ex.Message;
                        }
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoFail;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeUpdateE;
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = "Error al actualizar el usuario: " + ex.Message;
            }

            return new JsonResult(mensajeria);
        }
        public JsonResult DeleteUsuaio(int IdUsuario)
        {
            var usuarioExiste = _context.Usuarios.FirstOrDefault(x => x.IdUsuario == IdUsuario);

            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (usuarioExiste != null)
                    {
                        usuarioExiste.Bloqueado = false;

                        _context.SaveChanges();
                        context.Commit();
                        mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        mensajeria.mensajeDescripcion = MensajeExito.MensajeDelete;
                    }
                    else
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoFail;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE;
                    }
                }
                catch (Exception ex)
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoError;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE + ex.Message;
                    context.Rollback();
                }
            }
            return new JsonResult(mensajeria);
        }
    }
}

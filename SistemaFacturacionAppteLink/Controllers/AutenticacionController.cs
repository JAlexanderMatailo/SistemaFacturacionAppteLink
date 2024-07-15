using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SisCazhapugroBack.Utilidades;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.Services;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Controllers
{
    [Route("Facturacion/[controller]")]
    [ApiController]
    public class AutenticacionController : Controller
    {
        private readonly SistemaFacturacionContext _context;

        public AutenticacionController(SistemaFacturacionContext context)
        {
            _context = context;
        }
        [HttpPost("Loggin")]
        public IActionResult Authentication(UserLoginRequest credentials)
        {
            var encryptedPassword = credentials.Password; //Debe consumir el metodo de cifrado
            UsuariosVMResponse user = new UsuariosVMResponse();

            ServiceUsuarios usuarioLogic = new ServiceUsuarios(_context);
            var usuario = usuarioLogic.DatoUsuario(credentials.Nombre, encryptedPassword);

            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                //user = new UsuariosVMResponse()
                //{
                //    IdUsuario = usuario.IdUsuario,
                //    token = Token.GenerarToken(usuario.Usuario1),
                //    nombreUsuario = usuario.Usuario1,
                //    Correo = usuario.Correo
                //};
                return new JsonResult(usuario);
            }

            //return Ok(Token.GenerarToken(usuario.NameUser));
            //return new JsonResult(user);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Controllers
{
    [Route("Facturacion/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IUsuario _usuario;
        public UsuarioController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpPost("SetUsuarios")]
        public IActionResult SetUsuarios(UsuarioVMRequest usuario)
        {
            var result = _usuario.SetUsuarios(usuario);

            return new JsonResult(result.Value);

        }
        [HttpGet("GetUsersHist")]
        public IActionResult GetUsersHist()
        {
            var result = _usuario.GetUsersHist();
            return new JsonResult(result);
        }
        [HttpPost("UpdateUserPassword")]
        public IActionResult UpdateUserPassword(UsuarioVMRequest usuario)
        {
            var result = _usuario.UpdateUserPassword(usuario);

            return new JsonResult(result.Value);

        }
        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser(int IdUsuario, UsuarioVMRequest usuario)
        {
            var result = _usuario.UpdateUser(IdUsuario, usuario);

            return new JsonResult(result.Value);

        }
        [HttpPost("DeleteUsuaio")]
        public IActionResult DeleteCliente(int IdUsuario)
        {
            var result = _usuario.DeleteUsuaio(IdUsuario);

            return new JsonResult(result.Value);

        }
    }
}

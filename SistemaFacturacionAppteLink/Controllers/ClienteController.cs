using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.Infraestructura;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Controllers
{
    [Route("Facturacion/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly ICliente _cliente;
        public ClienteController(ICliente cliente)
        {
            _cliente = cliente;
        }

        [HttpPost("SetCliente")]
        public IActionResult Cliente(ClientesVM clientes)
        {
            var result = _cliente.SetClientes(clientes);

            return new JsonResult(result.Value);

        }
        [HttpGet("GetClientes")]
        public IActionResult GetClientes()
        {
            var result = _cliente.GetClientesHistory();
            return new JsonResult(result);
        }
        [HttpPost("UpdateCliente")]
        public IActionResult UpdateCliente(ClientesVM cliente)
        {
            var result = _cliente.UpdateCliente(cliente);

            return new JsonResult(result.Value);

        }
        [HttpPost("DeleteCliente")]
        public IActionResult DeleteCliente(int idCliente)
        {
            var result = _cliente.DeleteCliente(idCliente);

            return new JsonResult(result.Value);

        }

    }
}

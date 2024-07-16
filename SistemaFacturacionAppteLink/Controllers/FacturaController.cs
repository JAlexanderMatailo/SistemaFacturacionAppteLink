using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Controllers
{
    [Route("Facturacion/[controller]")]
    [ApiController]
    public class FacturaController : Controller
    {
        private readonly IFactura _factura;
        public FacturaController(IFactura factura)
        {
            _factura = factura;
        }

        [HttpPost("CrearFacturaAsync")]
        public IActionResult CrearFacturaAsync(FacturaVMRequest factura)
        {
            var result = _factura.CrearFactura(factura);

            return new JsonResult(result);

        }
        [HttpGet("GetFacturas")]
        public IActionResult GetFacturas()
        {
            var result = _factura.GetFacturas();
            return new JsonResult(result);
        }
        [HttpGet("GenerarNumeroFactura")]
        public IActionResult GenerarNumeroFactura()
        {
            var result = _factura.GenerarNumeroFactura();
            return new JsonResult(result);
        }

        [HttpPost("DeleteFactura")]
        public IActionResult DeleteFactura(Eliminacion factura)
        {
            var result = _factura.DeleteFactura(factura);

            return new JsonResult(result);

        }
        
    }
}

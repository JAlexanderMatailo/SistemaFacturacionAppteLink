using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Controllers
{
    [Route("Facturacion/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly IProducto _producto;

        public ProductoController(IProducto producto)
        {
            _producto = producto;
        }

        [HttpPost("SetProductos")]
        public IActionResult SetProductos(ProductoVMRequest producto)
        {
            var result = _producto.SetProductos(producto);

            return new JsonResult(result.Value);

        }
        [HttpGet("GetProductosHistory")]
        public IActionResult GetProductosHistory()
        {
            var result = _producto.GetProductosHistory();
            return new JsonResult(result);
        }
        [HttpPost("UpdateProduct")]
        public IActionResult UpdateProduct(ProductoVMRequest producto)
        {
            var result = _producto.UpdateProduct(producto);

            return new JsonResult(result.Value);

        }
        [HttpPost("DeleteProducto")]
        public IActionResult DeleteProducto(int IdProducto)
        {
            var result = _producto.DeleteProducto(IdProducto);

            return new JsonResult(result.Value);

        }
        
        [HttpPost("ReactivarProducto")]
        public IActionResult ReactivarProducto(string Codigo)
        {
            var result = _producto.ReactivarProducto(Codigo);

            return new JsonResult(result.Value);

        }
    }
}

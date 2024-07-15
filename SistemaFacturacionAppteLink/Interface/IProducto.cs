using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Interface
{
    public interface IProducto
    {
        JsonResult SetProductos(ProductoVMRequest productoVM);
        ResultProductos GetProductosHistory();
        JsonResult UpdateProduct(ProductoVMRequest producto);
        JsonResult DeleteProducto(int IdProducto);
        JsonResult ReactivarProducto(string Codigo);
    }
}

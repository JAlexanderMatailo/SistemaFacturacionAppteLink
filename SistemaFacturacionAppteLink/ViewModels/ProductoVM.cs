namespace SistemaFacturacionAppteLink.ViewModels
{
    public class ProductoVMResponse
    {
        public int IdProducto { get; set; }

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
    public class ProductoVMRequest
    {
        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }

    public class ResultProductos : MensajesVM
    {
        public List<ProductoVMResponse> listProductos { get; set; }
    }
}

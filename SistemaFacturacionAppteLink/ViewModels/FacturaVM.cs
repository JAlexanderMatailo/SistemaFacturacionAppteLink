namespace SistemaFacturacionAppteLink.ViewModels
{
    //public class FacturaVMRequest
    //{
    //    public int IdFactura { get; set; }
    //    public string NumeroFactura { get; set; } = null!;

    //    public int IdCliente { get; set; }

    //    public decimal Subtotal { get; set; }

    //    public decimal Igv { get; set; }

    //    public decimal Total { get; set; }

    //    public string CodigoProducto { get; set; } = null!;

    //    public string NombreProducto { get; set; } = null!;

    //    public decimal Precio { get; set; }

    //    public int Cantidad { get; set; }

    //    public decimal SubtotalF { get; set; }

    //}

    //public class FacturaResponse
    //{
    //    public FacturaVMRequest Factura { get; set; }
    //    public MensajesVM Mensajeria { get; set; }
    //}
    public class FacturaVMRequest
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } = null!;
        public int IdCliente { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        
        public List<ProductoFacturaVM> Productos { get; set; } = new List<ProductoFacturaVM>();
    }

    public class ProductoFacturaVM
    {
        public string CodigoProducto { get; set; } = null!;
        public string NombreProducto { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal SubtotalF { get; set; }
    }

    public class FacturaResponse
    {
        public FacturaVMRequest Factura { get; set; }
        public MensajesVM Mensajeria { get; set; }
    }

    public class FacturaVMResponse
    {
        public int IdFactura { get; set; }

        public string NumeroFactura { get; set; } = null!;

        public int IdCliente { get; set; }

        public decimal Subtotal { get; set; }

        public decimal PorcentajeIgv { get; set; }

        public decimal Igv { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool? Activo { get; set; }

        public int IdItem { get; set; }

        public string CodigoProducto { get; set; } = null!;

        public string NombreProducto { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }
        public decimal SubtotalF { get; set; }

        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Correo { get; set; } = null!;

    }

    public class ResultFactura : MensajesVM
    {
        public List<FacturaVMResponse> FacturaList { get; set;}
    }


    public class Eliminacion
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } = null!;
    }
    
    public class EliminacionFactura : MensajesVM
    {
        public Eliminacion eliminar { get; set; }
    }
}

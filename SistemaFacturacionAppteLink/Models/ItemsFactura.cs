using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class ItemsFactura
{
    public int IdItem { get; set; }

    public int IdFactura { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public decimal SubtotalF { get; set; }

    public virtual Producto CodigoProductoNavigation { get; set; } = null!;

    public virtual Factura IdFacturaNavigation { get; set; } = null!;
}

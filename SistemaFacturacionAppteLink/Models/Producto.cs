using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<ItemsFactura> ItemsFacturas { get; set; } = new List<ItemsFactura>();
}

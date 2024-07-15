using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public int IdCliente { get; set; }

    public decimal Subtotal { get; set; }

    public decimal PorcentajeIgv { get; set; }

    public decimal Igv { get; set; }

    public decimal Total { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual ICollection<ItemsFactura> ItemsFacturas { get; set; } = new List<ItemsFactura>();
}

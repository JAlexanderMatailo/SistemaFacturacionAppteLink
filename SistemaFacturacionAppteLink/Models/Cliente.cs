using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string RucDni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}

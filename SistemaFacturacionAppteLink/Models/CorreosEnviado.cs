using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class CorreosEnviado
{
    public int IdCorreo { get; set; }

    public int IdUsuario { get; set; }

    public string Correo { get; set; } = null!;

    public string CodigoTemporal { get; set; } = null!;

    public string CuerpoCorreo { get; set; } = null!;

    public DateTime? FechaEnvio { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

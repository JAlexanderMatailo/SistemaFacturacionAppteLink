using System;
using System.Collections.Generic;

namespace SistemaFacturacionAppteLink.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int? IntentosFallidos { get; set; }

    public bool? Bloqueado { get; set; }

    public virtual ICollection<CorreosEnviado> CorreosEnviados { get; set; } = new List<CorreosEnviado>();
}

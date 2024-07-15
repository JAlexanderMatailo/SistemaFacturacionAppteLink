using SistemaFacturacionAppteLink.Models;

namespace SistemaFacturacionAppteLink.ViewModels
{
    public class ClientesVM
    {
        public string RucDni { get; set; } 

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Correo { get; set; }
    }
    public partial class ClienteResponse
    {
        public int IdCliente { get; set; }

        public string RucDni { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? Direccion { get; set; }

        public string? Correo { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaCreacion { get; set; }

    }

    public class ResultClientes : MensajesVM
    {
        public List<ClienteResponse> listClientes { get; set; }
    }
}

namespace SistemaFacturacionAppteLink.ViewModels
{
    public class UsuarioVMRequest
    {
        public string Usuario1 { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string Correo { get; set; } = null!;

    }
    public class UsuarioVMResponse
    {
        public int IdUsuario { get; set; }

        public string Usuario1 { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public int? IntentosFallidos { get; set; }

        public bool? Bloqueado { get; set; }
    }

    public class ResultUsers : MensajesVM
    {
        public List<UsuarioVMResponse> ListUsuarios { get; set; }
    }
    public class ResultUsersLogin : MensajesVM
    {
        public UsuariosVMResponse Usuario { get; set; }
    }
    public class UserLoginRequest
    {
        public string Nombre { get; set; }
        public string Password { get; set; }
    }
    public class UsuariosVMResponse
    {
        public int IdUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string token { get; set; }
        public string Correo { get; set; }
    }
}

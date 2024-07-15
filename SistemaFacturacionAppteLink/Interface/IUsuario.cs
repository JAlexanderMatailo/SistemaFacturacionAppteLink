using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Interface
{
    public interface IUsuario
    {
        JsonResult SetUsuarios(UsuarioVMRequest usuario);
        ResultUsers GetUsersHist();
        JsonResult UpdateUserPassword(UsuarioVMRequest usuarios);
        JsonResult UpdateUser(int IdUsuario, UsuarioVMRequest usuarios);
        JsonResult DeleteUsuaio(int IdUsuario);
    }
}

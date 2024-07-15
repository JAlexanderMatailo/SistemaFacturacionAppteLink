using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Interface
{
    public interface ICliente
    {
        JsonResult SetClientes(ClientesVM clientes);
        ResultClientes GetClientesHistory();
        JsonResult UpdateCliente(ClientesVM cliente);
        JsonResult DeleteCliente(int idCliente);
    }
}

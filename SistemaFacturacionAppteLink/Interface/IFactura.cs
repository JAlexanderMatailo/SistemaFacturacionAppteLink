using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Interface
{
    public interface IFactura
    {
        FacturaResponse CrearFactura(FacturaVMRequest factura);
        ResultFactura GetFacturas();
        EliminacionFactura DeleteFactura(Eliminacion factura);
    }
}

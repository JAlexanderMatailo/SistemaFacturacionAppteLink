using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacionAppteLink.Infraestructura;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Services
{
    public class ServiceFacturas : IFactura
    {
        MensajesVM mensajeria = new MensajesVM();
        private readonly SistemaFacturacionContext _context;

        public ServiceFacturas(SistemaFacturacionContext context)
        {
            _context = context;
        }
        public string GenerarNumeroFactura()
        {
            var configuracion = _context.Configuracions.First();
            configuracion.UltimoNumeroFactura += 1;
            _context.SaveChanges();
            return configuracion.UltimoNumeroFactura.ToString();
        }

        public FacturaResponse CrearFactura(FacturaVMRequest factura)
        {
            factura.NumeroFactura = GenerarNumeroFactura();
            var facturaExiste = _context.Facturas.Any(x => x.NumeroFactura == factura.NumeroFactura);

            try
            {
                if (!facturaExiste)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Factura nuevaFactura = new Factura()
                            {
                                NumeroFactura = factura.NumeroFactura,
                                IdCliente = factura.IdCliente,
                                Subtotal = factura.Subtotal,
                                Igv = factura.Igv,
                                Total = factura.Total
                            };

                            _context.Facturas.Add(nuevaFactura);
                            _context.SaveChangesAsync();

                            // Obtener el Id de la factura recién guardada
                            int idFacturaGuardada = nuevaFactura.IdFactura;

                            ItemsFactura ifactura = new ItemsFactura()
                            {
                                IdFactura = idFacturaGuardada, // Asignar el Id de la factura guardada
                                CodigoProducto = factura.CodigoProducto,
                                NombreProducto = factura.NombreProducto,
                                Precio = factura.Precio,
                                Cantidad = factura.Cantidad,
                                SubtotalF = factura.SubtotalF
                            };

                            _context.ItemsFacturas.Add(ifactura);
                            _context.SaveChangesAsync();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = "Factura creada exitosamente.";
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoError;
                            mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeError + ex.Message;
                            context.Rollback();
                        }
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoError;
                    mensajeria.mensajeDescripcion = "El número de factura ya existe.";
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeNoConexion + ex.Message;
            }

            return new FacturaResponse
            {
                Factura = factura,
                Mensajeria = mensajeria
            };
        }


        public ResultFactura GetFacturas()
        {
            ResultFactura result = new ResultFactura()
            {
                FacturaList = new List<FacturaVMResponse>()
            };

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var facturas = (
                            from itF in _context.ItemsFacturas
                            join fact in _context.Facturas on itF.IdFactura equals fact.IdFactura
                            join cli in _context.Clientes on fact.IdCliente equals cli.IdCliente
                            where (fact.Activo == true)
                            select new
                            {
                                itF.IdItem,
                                itF.CodigoProducto,
                                itF.NombreProducto,
                                itF.Precio,
                                itF.Cantidad,
                                itF.SubtotalF,
                                fact.IdFactura,
                                fact.NumeroFactura,
                                fact.Subtotal,
                                fact.PorcentajeIgv,
                                fact.Igv,
                                fact.Total,
                                fact.FechaCreacion,
                                fact.Activo,
                                cli.IdCliente,
                                cli.RucDni,
                                cli.Nombre,
                                cli.Direccion,
                                cli.Correo
                            }
                        ).ToList();

                        if (facturas != null && facturas.Any())
                        {
                            foreach (var fact in facturas)
                            {
                                FacturaVMResponse factura = new FacturaVMResponse()
                                {
                                    IdFactura = fact.IdFactura,
                                    NumeroFactura = fact.NumeroFactura,
                                    IdCliente = fact.IdCliente,
                                    Subtotal = fact.Subtotal,
                                    PorcentajeIgv = fact.PorcentajeIgv,
                                    Igv = fact.Igv,
                                    Total = fact.Total,
                                    FechaCreacion = (DateTime)fact.FechaCreacion,
                                    Activo = fact.Activo,
                                    IdItem = fact.IdItem,
                                    CodigoProducto = fact.CodigoProducto,
                                    NombreProducto = fact.NombreProducto,
                                    Precio = fact.Precio,
                                    Cantidad = fact.Cantidad,
                                    SubtotalF = fact.SubtotalF,
                                    Nombre = fact.Nombre,
                                    Direccion = fact.Direccion,
                                    Correo = fact.Correo
                                };
                                result.FacturaList.Add(factura);
                            }
                            result.codigoResult = (int)Codigos.CodigoSuccess;
                            result.mensajeDescripcion = MensajeExito.MensajeSucces;
                        }
                        else
                        {
                            result.codigoResult = (int)Codigos.CodigoFail;
                            result.mensajeDescripcion = MensajeExcepciones.MensajeExisteRegistro;
                        }

                        transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        result.codigoResult = (int)Codigos.CodigoError;
                        result.mensajeDescripcion = "Error de consulta: " + ex.Message;
                        transaction.RollbackAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                result.codigoResult = (int)Codigos.CodigoError;
                result.mensajeDescripcion = "Error al consultar usuarios: " + ex.Message;
            }

            return result;
        }

        public EliminacionFactura DeleteFactura(Eliminacion factura)
        {
            EliminacionFactura elFactura = new EliminacionFactura();
            var facturaExiste = _context.Facturas.FirstOrDefault(x => x.IdFactura == factura.IdFactura && x.NumeroFactura == factura.NumeroFactura);
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (facturaExiste != null)
                    {
                        facturaExiste.Activo = false;

                        _context.SaveChanges();
                        transaction.Commit();
                        elFactura.eliminar.IdFactura = facturaExiste.IdFactura;
                        elFactura.eliminar.NumeroFactura = facturaExiste.NumeroFactura;
                        elFactura.codigoResult = mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        elFactura.mensajeDescripcion = mensajeria.mensajeDescripcion = MensajeExito.MensajeDeleteFactura;
                    }
                    else
                    {
                        elFactura.codigoResult = mensajeria.codigoResult = (int)Codigos.CodigoFail;
                        elFactura.mensajeDescripcion = mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteF;
                    }
                }
                catch (Exception ex)
                {
                    elFactura.codigoResult = mensajeria.codigoResult = (int)Codigos.CodigoError;
                    elFactura.mensajeDescripcion = mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteF + ex.Message;
                    transaction.Rollback();
                }
            }
            return elFactura;
        }

    }
}

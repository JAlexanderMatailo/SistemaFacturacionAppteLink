using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaFacturacionAppteLink.Infraestructura;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;

namespace SistemaFacturacionAppteLink.Services
{
    public class ServiceProductos : IProducto
    {
        MensajesVM mensajeria = new MensajesVM();
        SistemaFacturacionContext _context;
        public ServiceProductos(SistemaFacturacionContext context)
        {
            _context = context;
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }  
            return string.Concat(texto.ToLower().Where(c => !char.IsWhiteSpace(c)));
        }
        private bool ProductoExistePorNombre(string nombreProducto)
        {
            string descripcionNormalizada = NormalizarTexto(nombreProducto);

            var descripcionesExistentes = _context.Productos.ToList()
                .Select(x => new { x.Nombre })
                .AsEnumerable()
                .ToList();
            var existe = descripcionesExistentes.Any(x => NormalizarTexto(x.Nombre) == descripcionNormalizada);
            return existe;
        }
        public JsonResult SetProductos(ProductoVMRequest productoVM)
        {
            var existeProducto = _context.Productos.Where(x => x.Codigo == productoVM.Codigo).Any();
            var existeStockProducto = _context.Productos.FirstOrDefault(x => x.Codigo == productoVM.Codigo && x.Nombre == productoVM.Nombre);
            try
            {
                if (!existeProducto && !ProductoExistePorNombre(productoVM.Nombre))
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Producto producto = new Producto()
                            {
                                Codigo = productoVM.Codigo,
                                Nombre = productoVM.Nombre,
                                Precio = productoVM.Precio,
                                Stock = 1,
                                Activo = true,
                                FechaCreacion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            };
                            _context.Productos.Add(producto);
                            _context.SaveChanges();
                            context.Commit();
                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeRegistro;
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
                    if(!existeProducto && existeStockProducto == null)
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeExisteProducto;
                    }
                    else
                    {
                        if (existeStockProducto != null && existeStockProducto.Codigo != null)
                        {
                            UpdateStock(existeStockProducto.Codigo, existeStockProducto.Stock);
                        }
                        else
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                            mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeExisteProducto;
                        }

                    }
                    
                }
            }catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeNoConexion + ex.Message;
            }
            

            return new JsonResult(mensajeria);
        }
        public JsonResult UpdateStock(string Codigo, int stock)
        {
            try
            {
                var ProductoExiste = _context.Productos.Where(x => x.Codigo == Codigo && x.Activo == true).FirstOrDefault();
                if (ProductoExiste != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProductoExiste.Stock = stock + 1;                           

                            _context.SaveChanges();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeUpdateStockProduct;
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = "Error al conectar al servicio para la actualizacion de datos" + ex.Message;
                        }
                    }
                }
                else
                {
                    ReactivarProducto(Codigo);
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = "Error al actualizar el producto: " + ex.Message;
            }

            return new JsonResult(mensajeria);
        }
        public ResultProductos GetProductosHistory()
        {
            ResultProductos result = new ResultProductos
            {
                listProductos = new List<ProductoVMResponse>()
            };

            try
            {
                var productosT = _context.Productos.ToList();
                foreach (var producto in productosT)
                {
                    ProductoVMResponse productoL = new ProductoVMResponse
                    {
                        IdProducto = producto.IdProducto,
                        Codigo = producto.Codigo,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Stock = producto.Stock,
                        FechaCreacion = producto.FechaCreacion,
                        Activo = producto.Activo
                    };
                    result.listProductos.Add(productoL);
                }
                if(result.listProductos.Count > 0)
                {
                    result.codigoResult = (int)Codigos.CodigoSuccess;
                    result.mensajeDescripcion = MensajeExito.MensajeSucces;
                }
                else
                {
                    result.codigoResult = (int)Codigos.CodigoFail;
                    result.mensajeDescripcion = MensajeExcepciones.MensajeExisteRegistro;
                }
                
            }
            catch (Exception ex)
            {
                result.codigoResult = (int)Codigos.CodigoError;
                result.mensajeDescripcion = "Error al consultar productos: " + ex.Message;
            }

            return result;
        }
        public JsonResult UpdateProduct(ProductoVMRequest producto)
        {
            // Lógica para actualizar el cliente
            try
            {
                var ProductoExiste = _context.Productos.Where(x => x.Codigo == producto.Codigo).FirstOrDefault();

                if (ProductoExiste != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProductoExiste.Nombre = producto.Nombre;
                            ProductoExiste.Precio = producto.Precio;

                            _context.SaveChanges();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeUpdate;
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = "Error al conectar al servicio para la actualizacion de datos" + ex.Message;
                            context.Rollback();
                        }
                    }

                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoFail;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeUpdateE;
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = "Error al actualizar el producto: " + ex.Message;
            }

            return new JsonResult(mensajeria);
        }
        public JsonResult DeleteProducto(int IdProducto)
        {
            MensajesVM mensajeria = new MensajesVM();
            var productoExiste = _context.Productos.FirstOrDefault(x => x.IdProducto == IdProducto);

            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (productoExiste != null)
                    {
                        productoExiste.Activo = false;

                        _context.SaveChanges();
                        context.Commit();
                        mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        mensajeria.mensajeDescripcion = MensajeExito.MensajeDelete;
                    }
                    else
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoFail;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE;
                    }
                }
                catch (Exception ex)
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoError;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE + ex.Message;
                    context.Rollback();
                }
            }
            return new JsonResult(mensajeria);
        }
        public JsonResult ReactivarProducto(string Codigo)
        {
            MensajesVM mensajeria = new MensajesVM();
            var productoExiste = _context.Productos.FirstOrDefault(x => x.Codigo == Codigo);

            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (productoExiste != null)
                    {
                        productoExiste.Activo = true;

                        _context.SaveChanges();
                        context.Commit();
                        mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        mensajeria.mensajeDescripcion = MensajeExito.MensajeActivo;
                    }
                    else
                    {
                        mensajeria.codigoResult = (int)Codigos.CodigoFail;
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE;
                    }
                }
                catch (Exception ex)
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoError;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeDeleteE + ex.Message;
                    context.Rollback();
                }
            }
            return new JsonResult(mensajeria);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionAppteLink.Infraestructura;
using SistemaFacturacionAppteLink.Interface;
using SistemaFacturacionAppteLink.Models;
using SistemaFacturacionAppteLink.ViewModels;
using Newtonsoft.Json;

namespace SistemaFacturacionAppteLink.Services
{
    public class ServiceClientes : ICliente
    {
        SistemaFacturacionContext _context;
        public ServiceClientes(SistemaFacturacionContext context)
        {
            _context = context;
        }
        private bool EsCedulaEcuatorianaValida(string cedula)
        {
            if (cedula.Length != 10 || !cedula.All(char.IsDigit))
            {
                return false;
            }

            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int total = 0;

            for (int i = 0; i < 9; i++)
            {
                int valor = (cedula[i] - '0') * coeficientes[i];
                if (valor >= 10)
                {
                    valor -= 9;
                }
                total += valor;
            }

            int digitoVerificador = (10 - (total % 10)) % 10;
            return digitoVerificador == (cedula[9] - '0');
        }
        public bool EsCedulaValida(string cedula)
        {
            if (EsCedulaEcuatorianaValida(cedula))
            {
                return true;
            }

            return false;
        }
        public JsonResult SetClientes(ClientesVM clientes)
        {
            MensajesVM mensajeria = new MensajesVM();
            bool registro = false;
            bool cedula = false;
            var existeDni = _context.Clientes.Where(x => x.RucDni == clientes.RucDni).Any();
            try
            {
                if (!existeDni && clientes.RucDni.Length >= 10)
                {
                    cedula = EsCedulaValida(clientes.RucDni);
                    if (cedula)
                    {
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeSuccesCedula;
                    }
                    else
                    {
                        mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeSuccesRuc;
                    }
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Cliente clientesA = new Cliente
                            {
                                RucDni = clientes.RucDni,
                                Nombre = clientes.Nombre,
                                Direccion = clientes.Direccion,
                                FechaCreacion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                                Correo = clientes.Correo,
                                Activo = true,
                            };
                            _context.Clientes.Add(clientesA);
                            _context.SaveChanges();
                            context.Commit();
                            registro = true;
                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                        }
                        catch (Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoErrorServer;
                            mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeNoConexion + ex.Message;
                            context.Rollback();
                            registro = false;
                        }
                    }
                }
                else
                {
                    mensajeria.codigoResult = (int)Codigos.CodigoExisteDni;
                    mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeExisteCedula;
                }
            }
            catch (Exception ex)
            {
                mensajeria.codigoResult = (int)Codigos.CodigoError;
                mensajeria.mensajeDescripcion = MensajeExcepciones.MensajeNoConexion + ex.Message;

            }

            return new JsonResult(mensajeria);
        }

        public ResultClientes GetClientesActive()
        {
            ResultClientes result = new ResultClientes
            {
                listClientes = new List<ClienteResponse>()
            };

            try
            {
                var clientesT = _context.Clientes.Where(x => x.Activo == true).ToList();
                foreach (var cliente in clientesT)
                {
                    ClienteResponse clienteL = new ClienteResponse
                    {
                        IdCliente = cliente.IdCliente,
                        RucDni = cliente.RucDni,
                        Nombre = cliente.Nombre,
                        Direccion = cliente.Direccion,
                        Correo = cliente.Correo,
                        Activo = cliente.Activo,
                        FechaCreacion = cliente.FechaCreacion
                    };
                    result.listClientes.Add(clienteL);
                }

                result.codigoResult = (int)Codigos.CodigoSuccess;
                result.mensajeDescripcion = MensajeExito.MensajeSucces;
            }
            catch (Exception ex)
            {
                result.codigoResult = (int)Codigos.CodigoError;
                result.mensajeDescripcion = "Error al consultar clientes: " + ex.Message;
            }

            return result;
        }
        public ResultClientes GetClientesNoActive()
        {
            ResultClientes result = new ResultClientes
            {
                listClientes = new List<ClienteResponse>()
            };

            try
            {
                var clientesT = _context.Clientes.Where(x => x.Activo == false).ToList();
                foreach (var cliente in clientesT)
                {
                    ClienteResponse clienteL = new ClienteResponse
                    {
                        IdCliente = cliente.IdCliente,
                        RucDni = cliente.RucDni,
                        Nombre = cliente.Nombre,
                        Direccion = cliente.Direccion,
                        Correo = cliente.Correo,
                        Activo = cliente.Activo,
                        FechaCreacion = cliente.FechaCreacion
                    };
                    result.listClientes.Add(clienteL);
                }

                result.codigoResult = (int)Codigos.CodigoSuccess;
                result.mensajeDescripcion = MensajeExito.MensajeSucces;
            }
            catch (Exception ex)
            {
                result.codigoResult = (int)Codigos.CodigoError;
                result.mensajeDescripcion = "Error al consultar clientes: " + ex.Message;
            }

            return result;
        }
        public ResultClientes GetClientesHistory()
        {
            ResultClientes result = new ResultClientes
            {
                listClientes = new List<ClienteResponse>()
            };

            try
            {
                var clientesT = _context.Clientes.ToList();
                foreach (var cliente in clientesT)
                {
                    ClienteResponse clienteL = new ClienteResponse
                    {
                        IdCliente = cliente.IdCliente,
                        RucDni = cliente.RucDni,
                        Nombre = cliente.Nombre,
                        Direccion = cliente.Direccion,
                        Correo = cliente.Correo,
                        Activo = cliente.Activo,
                        FechaCreacion = cliente.FechaCreacion
                    };
                    result.listClientes.Add(clienteL);
                }
                if(result.listClientes.Count > 0)
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
                result.mensajeDescripcion = "Error al consultar clientes: " + ex.Message;
            }

            return result;
        }

        public JsonResult UpdateCliente(ClientesVM cliente)
        {
            MensajesVM mensajeria = new MensajesVM();

            // Lógica para actualizar el cliente
            try
            {
                var ClienteExiste = _context.Clientes.Where(x => x.RucDni == cliente.RucDni).FirstOrDefault();

                if (ClienteExiste != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            ClienteExiste.Nombre = cliente.Nombre;
                            ClienteExiste.Direccion = cliente.Direccion;
                            ClienteExiste.Correo = cliente.Correo;

                            _context.SaveChanges();
                            context.Commit();

                            mensajeria.codigoResult = (int)Codigos.CodigoSuccess;
                            mensajeria.mensajeDescripcion = MensajeExito.MensajeUpdate;
                        }
                        catch(Exception ex)
                        {
                            mensajeria.codigoResult = (int)Codigos.CodigoFail;
                            mensajeria.mensajeDescripcion = "Error al conectar al servicio para la actualizacion de datos" + ex.Message;
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
                mensajeria.mensajeDescripcion = "Error al actualizar el cliente: " + ex.Message;
            }

            return new JsonResult(mensajeria);
        }

        public JsonResult DeleteCliente(int idCliente)
        {
            MensajesVM mensajeria = new MensajesVM();
            var clienteExiste = _context.Clientes.FirstOrDefault(x => x.IdCliente == idCliente);

            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (clienteExiste != null)
                    {
                        clienteExiste.Activo = false;

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

    }
}

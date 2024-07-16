namespace SistemaFacturacionAppteLink.Infraestructura
{
    public static class MensajeExcepciones
    {
        public const string MensajeSuccesRuc = "Registro con Ruc exitoso";
        public const string MensajeError = "No se pudo concretar el registro";
        public const string MensajeNoConexion = "No se pudo conectar al servicio";
        public const string MensajeSuccesCedula = "Registro con cedula exitoso";
        public const string MensajeExisteCedula = "Ya existe un cliente registrado con este DNI";
        public const string MensajeUpdateE = "No se pudo actualizar los datos";
        public const string MensajeDeleteE = "No se pudo eliminar, debido a que no existe";
        public const string MensajeDeleteF = "No se pudo eliminar la factura, debido a que no existe";
        public const string MensajeExisteUsuario = "Ya existe un usuario con este nombre, ingrese otro";
        public const string MensajeExisteUsuarioCorreo = "El correo ingresado está asociado a otro usuario";
        public const string MensajeExisteRegistro = "Los sentimos, pero aún no existen registros";
        public const string MensajeExisteProducto= "Los sentimos,revise los datos ingresados ya que puede existir un producto con el mismo nombre y diferente codigo o con el mismo codigo y diferente nombre.";
        public const string MensajeIPassword= "Contraseña incorrecta";
        public const string MensajeIUser= "Usuario incorrecto";
        public const string MensajeBUser= "Usuario bloqueado";

    }
    public static class MensajeExito
    {
        public const string MensajeSucces = "Se obtuvo los datos de forma correcta";
        public const string MensajeUpdate = "Datos actualizados correctamente";
        public const string MensajeUpdateStockProduct = "Stock del producto ingresado ha sido actualizado";
        public const string MensajeDelete = "Se eliminó de forma correcta al cliente";
        public const string MensajeActivo = "Producto reactivado";
        public const string MensajeDeleteFactura = "Se elimino la factura de forma correcta";
        public const string MensajeRegistro = "Se registro de forma exitosa";
        public const string MensajeAutorizacion = "Usuario Autorizado";
    }
}

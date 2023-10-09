using FirebirdSql.Data.FirebirdClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class RepositorioFacturacionElectronica
    {
        //Obtener Informacion Clientes

        public DataTable ListarClientes()
        {
            DataTable tabla = new DataTable();  
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT Identificacion,RazonSocial,Direccion,Telefono,Email, Rut, c.Nombre, Fecha,Estado FROM" +
                                " T_Clientes inner join T_Ciudades c on IdCiudad=c.Id where estado=0");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }

        public string ActualizaClientes()
        {
            string rta = "";
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("update T_Clientes SET Estado=1 WHERE Estado=0");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
            return rta;

        }


        //public DataTable ListarPagos()
        //{
        //    DataTable tabla = new DataTable();
        //    SqlConnection sqlcon = new SqlConnection();
        //    try
        //    {
        //        sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
        //        string cadena = ("SELECT  P.Id, C.Empresa, p.FechaPago,c.Identificacion,c.CodigoSucursal,p.Prefijo,p.NumeroFactura, p.Total,p.IdEstacionamiento, tp.IdTipoPago, tp.TipoPago, c.Vendedor FROM T_Clientes C INNER JOIN " +
        //                        " T_Pagos P on c.Identificacion = p.NumeroDocumento INNER JOIN T_TipoPago tp on tp.IdTipoPago = p.IdTipoPago WHERE p.Estado = 0");
        //        SqlCommand comando = new SqlCommand(cadena, sqlcon);
        //        sqlcon.Open();
        //        SqlDataReader rta = comando.ExecuteReader();
        //        tabla.Load(rta);
        //        return tabla;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
        //    }
        //}

        public string ActualizaEstadoPagos(int id)
        {
            string rta = "";
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_Pagos SET Estado=1 WHERE Estado=0 and Id=" + id + "");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
            return rta;
        }

        //INTERFAZ

        #region Cliente
        public string InsertarCliente(Cliente cliente)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                    "CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE)VALUES ("+cliente.Empresa+", "+cliente.Identificacion+", 1, "+cliente.RazonSocial+", "+cliente.Direccion+","+cliente.Telefono+", "+cliente.Email+"," +
                    " "+cliente.IdCiudad+", "+cliente.Vendedor+", NULL,NULL);");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = cadena;

            }
            catch (Exception ex)
            {

                rta = "ERROR";
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;

        }

        #region New 

        public string InsertarClienteInterfaz(Cliente cliente)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = $"INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                           $"CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE) " +
                           $"VALUES ('1', '{cliente.Identificacion}', 1, '{cliente.RazonSocial}', '{cliente.Direccion}', " +
                           $"'{cliente.Telefono}', '{cliente.Email}', '{cliente.Nombre}', {cliente.Vendedor}, NULL,NULL)";
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = cadena;

            }
            catch (Exception ex)
            {

                rta = "ERROR";
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;
        }
        public DataTable ValidarExisteCliente(Cliente cliente)
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM CLIENTES WHERE CLI_IDENTIFICACION='" + cliente.Identificacion + "'");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return tabla;
        }

        //OLD

        //public string ActualizaEstadoCliente(int identificacion)
        //{
        //    string rta = "";
        //    SqlConnection sqlCon = new SqlConnection();
        //    try
        //    {
        //        sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
        //        string cadena = ("UPDATE T_CLIENTES SET ESTADO=1 WHERE Identificacion=" + identificacion + "");
        //        SqlCommand comando = new SqlCommand(cadena, sqlCon);
        //        sqlCon.Open();
        //        comando.ExecuteNonQuery();
        //        rta = "OK";

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
        //    }
        //    return rta;
        //}

        public string ActualizaEstadoCliente(Cliente cliente)
        {
            string rta = "";
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_CLIENTES SET ESTADO=1 WHERE Identificacion=" + cliente.Identificacion + "");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rta;
        }

        #endregion


        #endregion

        #region Pagos

        public string InsertarPagos(string texto)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = (texto);
                FbCommand comando = new FbCommand(texto, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";

            }
            catch (Exception ex)
            {

                rta = ex.ToString();
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;
        }

        public string InsertarCotizacionesEncabezado(string codigo)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = (codigo);
                FbCommand comando = new FbCommand(codigo, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;

        }


        #region New

        public DataTable ListarPagos()
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT      P.Id,      C.Empresa,      P.FechaPago,    " +
                    "  C.Identificacion,      C.CodigoSucursal,      P.Prefijo,      P.NumeroFactura,    " +
                    "  P.Total,      P.IdEstacionamiento,      tp.IdTipoPago,      tp.TipoPago,     " +
                    " C.Vendedor  FROM T_Clientes C  INNER JOIN T_Pagos P ON C.Identificacion = P.NumeroDocumento  " +
                    "INNER JOIN T_TipoPago tp ON tp.IdTipoPago = P.IdTipoPago  WHERE P.Estado = 0  GROUP BY      P.Id,    " +
                    "  C.Empresa,      P.FechaPago,      C.Identificacion,      C.CodigoSucursal,      P.Prefijo,    " +
                    "  P.NumeroFactura,      P.Total,      P.IdEstacionamiento,      tp.IdTipoPago,      tp.TipoPago,      C.Vendedor");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }

        public DataTable ListarCentroCosto(Pagos pagos)
        {
            SqlConnection sqlCon = new SqlConnection();
            DataTable tabla = new DataTable();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT replace(ep.documentoempresa,'NCRP','') as CentroCosto FROM T_EmpresaParquearse EP WHERE IdEstacionamiento=" + pagos.IdEstacionamiento + "");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
        }

        public string ActualizaEstadoPagos(Pagos pagos)
        {
            string rta = "";
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_Pagos SET Estado=1 WHERE Estado=0 and Id=" + pagos.Id + "");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
            return rta;
        }

        public string InsertarPagosInterfaz(Cotizaciones cotizaciones)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ($"INSERT INTO COTIZACIONES (COT_EMPRESA, COT_DOCUMENTO, COT_NUMERO, COT_ITEM, COT_TIPO_ITEM, COT_DESCRIPCION_ITEM, COT_REFERENCIA, COT_BODEGA," +
                                             $"  COT_CANTIDAD, COT_VALOR_UNITARIO, COT_VR_DTO, COT_FECHA_UPDATE, COT_CENTRO_COSTO, COT_PROYECTO)" +
                                             $" VALUES({cotizaciones.Cot_Empresa},'{cotizaciones.Cot_Documento}',{cotizaciones.Cot_Numero},{cotizaciones.Cot_Item},{cotizaciones.Cot_Tipo_Item},' ','{cotizaciones.Cot_Referencia}',NULL,{cotizaciones.Cot_Cantidad},{cotizaciones.Cot_Valor_Unitario},0,NULL,'{cotizaciones.Cot_Centro_Costo}',NULL);");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";
                GenerarArchivoPlano(cadena);

            }
            catch (Exception ex)
            {

                rta = ex.ToString();
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;
        }

        public string InsertaPagosCotizacionEncabezado(CotizacionEncabezado cotizacionEncabezado)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ($"INSERT INTO COTIZACION_ENCABEZADO (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                             $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR,COE_FORMA_PAGO)" +
                             $"VALUES({cotizacionEncabezado.Coe_Empresa},'{cotizacionEncabezado.Coe_Documento}',{cotizacionEncabezado.Coe_Numero},{cotizacionEncabezado.Coe_Fecha},{cotizacionEncabezado.Coe_Cliente}," +
                             $"{cotizacionEncabezado.Coe_Cliente_Sucursal},{cotizacionEncabezado.Coe_Sincronizado},NULL,'{cotizacionEncabezado.Coe_Observaciones}',NULL,NULL,0,NULL,NULL,NULL,'{cotizacionEncabezado.Coe_Vendedor}',{cotizacionEncabezado.Coe_Forma_Pago})");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";
                GenerarArchivoPlano(cadena);


            }
            catch (Exception ex)
            {

                rta = ex.ToString();
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;
        }

        public void GenerarArchivoPlano(string texto)
        {


            // Obtener la fecha actual para el nombre de archivo
            DateTime fechaActual = DateTime.Now;
            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registros-FE-" + fechaActual.Day.ToString() + "-" + fechaActual.Month.ToString() + "-" + fechaActual.Year.ToString());
            string nombre = "FacturaElectronica";
            string path = rutaFolder + "/" + nombre + @".txt";

            if (!Directory.Exists(rutaFolder))
            {
                Directory.CreateDirectory(rutaFolder);
            }

            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine(texto);
            }

        }


        #endregion

        //Obtener datos empresas 

        public DataTable ConsultarDatosContablesEmpresas()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT TOP(1) FROM EMPRESAS ORDER BY 1 ASC");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }

        public DataTable ListarUltimaCotizacion()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM COTIZACIONES WHERE COT_ITEM = (SELECT MAX(COT_ITEM) FROM COTIZACIONES)");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }

        //public DataTable ListarDatosEmpresasPorEstacionamiento(int idEstacionamiento)
        //{
        //    SqlConnection sqlCon = new SqlConnection();
        //    DataTable tabla = new DataTable();
        //    try
        //    {
        //        sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
        //        string cadena = (" SELECT Idc_Empresa, DocumentoEmpresa FROM T_EmpresaParquearse " +
        //                         "WHERE IdEstacionamiento=" + idEstacionamiento + "");
        //        SqlCommand comando = new SqlCommand(cadena, sqlCon);
        //        sqlCon.Open();
        //        SqlDataReader resultado = comando.ExecuteReader();
        //        tabla.Load(resultado);
        //        return tabla;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
        //    }
        //}

        public DataTable ListarDocumentoVendedor()
        {
            FbConnection fbCon = new FbConnection();
            DataTable tabla = new DataTable();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT VEN_IDENTIFICACION FROM VENDEDORES WHERE VEN_NOMBRE='PARQUEARSE S.A.S.'");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }

        #endregion

        #region FacturasContingencia

        public DataTable ListarFacturasContingencia()
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT " +
                    " P.IdPago,  C.Empresa,P.FechaPago, C.Identificacion,  " +
                    "    C.CodigoSucursal, P.Prefijo, " +
                    " P.NumeroFactura, P.Total, " +
                    " P.IdEstacionamiento, tp.IdTipoPago, " +
                    " tp.TipoPago,  C.Vendedor " +
                    " FROM T_Clientes C  INNER JOIN T_FacturasCOntingencia P ON C.Identificacion = P.IdentificacionCliente  INNER JOIN " +
                    " T_TipoPago tp ON tp.IdTipoPago = P.IdTipoPago  WHERE P.Sincronizacion = 0  GROUP BY  P.IdPago,  C.Empresa,   P.FechaPago, " +
                    " C.Identificacion, C.CodigoSucursal,  P.Prefijo,      P.NumeroFactura, " +
                    "P.Total,  P.IdEstacionamiento,      tp.IdTipoPago,  tp.TipoPago,  C.Vendedor");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }

        public DataTable ListarCentroCostoContingencia(FacturasContingencia facturasContingencia)
        {
            SqlConnection sqlCon = new SqlConnection();
            DataTable tabla = new DataTable();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT replace(ep.documentoempresa,'NCRP','') as CentroCosto FROM T_EmpresaParquearse EP WHERE IdEstacionamiento=" + facturasContingencia.IdEstacionamiento + "");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
        }



        public string ActualizaEstadoPagosContingencia(FacturasContingencia facturasContingencia)
        {
            string rta = "";
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_FacturasContingencia SET Sincronizacion=1 WHERE Sincronizacion=0 and IdPago=" + facturasContingencia.IdPago + "");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                comando.ExecuteNonQuery();
                rta = "OK";


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
            return rta;
        }


        #endregion

        //LISTADO DE TABLAS 

        public DataTable ListarCotizacionesEncabezado()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM COTIZACION_ENCABEZADO");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }
        public DataTable ListarCotizaciones()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM COTIZACIONES");
                FbCommand comando = new FbCommand(cadena,fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;


            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }
        public DataTable ListarClientesInterfaz()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM CLIENTES");
                FbCommand comando = new FbCommand(cadena,fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }
        //public DataTable ListarCentroCosto(int idEstacionamiento)
        //{
        //    SqlConnection sqlCon = new SqlConnection();
        //    DataTable tabla = new DataTable();
        //    try
        //    {
        //        sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
        //        string cadena = ("SELECT replace(ep.documentoempresa,'NCRP','') as CentroCosto FROM T_EmpresaParquearse EP WHERE IdEstacionamiento=" + idEstacionamiento + "");
        //       SqlCommand comando = new SqlCommand(cadena, sqlCon);
        //        sqlCon.Open();
        //        SqlDataReader rta = comando.ExecuteReader();
        //        tabla.Load(rta);
        //        return tabla;

        //    }
        //    catch (Exception ex )
        //    {

        //        throw ex ;
        //    }
        //    finally
        //    {
        //        if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
        //    }
        //}


        public DataTable ListarClientesNuevos()
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT TOP(1) * FROM T_CLIENTES WHERE ESTADO=0");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }
        public DataTable ListarClientesNuevosPorDoc(int identificacion)
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT TOP(1) * FROM T_CLIENTES WHERE ESTADO=0 AND Identificacion= "+identificacion+" ORDER BY FECHA DESC");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }




    }
}

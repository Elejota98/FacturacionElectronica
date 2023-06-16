﻿using FirebirdSql.Data.FirebirdClient;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                string cadena = ("SELECT Identificacion,RazonSocial,Direccion,Telefono,Email, c.Nombre, Fecha,Estado FROM" +
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


        public DataTable ListarPagos()
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT TOP(1) C.Empresa, c.Fecha,c.Identificacion,c.CodigoSucursal,p.Prefijo,p.NumeroFactura, c.Vendedor FROM T_Clientes C INNER JOIN" +
                    " T_Pagos P on c.Identificacion=p.NumeroDocumento  WHERE p.Estado=0");
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

        public string ActualizaEstadoPagos()
        {
            string rta = "";
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                sqlcon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_Pagos SET Estado=1 WHERE Estado=0");
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
        public DataTable ValidarExisteCliente(string documento)
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM CLIENTES WHERE CLI_IDENTIFICACION='" + documento + "'");
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


        public string InsertarClienteInterfaz(string texto)
        {
            string rta = "";
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
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
            catch (Exception ex )
            {

                rta = ex.ToString();
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return rta;
        }

        #endregion




    }
}

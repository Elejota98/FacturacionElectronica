using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class RepositorioNotaCredito
    {
        public DataTable ListarEstacionamientos()
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("Select * from T_Estacionamientos where Estado=1 order by nombre ");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;                   

            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if(sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
        }

        public DataTable ListarPagosInterfaz(int idEstacionamiento, string fechaPago)
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                string fechaConsultaInicio = fechaPago + " 00:00:00";
                string fechaConsultaFin = fechaPago + " 23:59:59";
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT cl.Identificacion, e.Nombre, SUM(Total) AS Total, FechaPago,  NumeroFactura " +
                    "FROM T_PagosFE INNER JOIN T_Clientes cl on T_PagosFE.Identificacion = cl.Identificacion INNER JOIN T_Estacionamientos e on T_PagosFE.IdEstacionamiento = e.IdEstacionamiento" +
                    " WHERE T_PagosFE.IdEstacionamiento=" + idEstacionamiento+" and FechaPago BETWEEN '"+fechaConsultaInicio+"' and '"+fechaConsultaFin+"' GROUP BY NumeroFactura, FechaPago, cl.Identificacion, e.Nombre");
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

        public DataTable ListarInterfaz(int idEstacionamiento, string numeroFactura, string fechaPago)
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                DateTime fechaFinal;
                fechaFinal = Convert.ToDateTime(fechaPago.ToString());
                fechaPago = fechaFinal.Year +"-"+ fechaFinal.Month +"-"+ fechaFinal.Day;
                string fechaFormateada = fechaFinal.ToString("yyyy-dd-MM");
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                SqlCommand comando = new SqlCommand("InterfazContableNC", sqlCon);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdEstacionamientoConsulta", SqlDbType.Int).Value = idEstacionamiento;
                comando.Parameters.Add("@FechaCosulta", SqlDbType.VarChar).Value = fechaFormateada;
                comando.Parameters.Add("@NumeroFactura", SqlDbType.Int).Value = numeroFactura;
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);    
                return tabla;

            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if(sqlCon.State==ConnectionState.Open) sqlCon.Close();
            }
        }


        //INTERFAZ

        public DataTable ListarItemsContable()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM ITEMSDOCCONTABLE");
                FbCommand comando = new FbCommand(cadena, fbCon);
                fbCon.Open();
                FbDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;
                    
            }
            catch (Exception ex )
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }

        public DataTable ListarDocContable()
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("SELECT * FROM DOCCONTABLE");
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

        public DataTable ValidarSiExisteElRegistro(DateTime fecha, int idcEmpresa, string idcDocumento, string numeroFactura)
        {
            DataTable tabla = new DataTable();
            FbConnection fbCon = new FbConnection();
            try
            {
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                string[] nuevaFecha = fechaFormateada.ToString().Split('-');
                DateTime MyDate = new DateTime(Convert.ToInt32(nuevaFecha[0]), Convert.ToInt32(nuevaFecha[2]), Convert.ToInt32(nuevaFecha[1]));
                double numeroFecha = MyDate.ToOADate();
                string numeroFechaSinEspacios = numeroFecha.ToString().Trim();

                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                    string cadena = "SELECT * FROM ITEMSDOCCONTABLE WHERE IDC_EMPRESA = " + idcEmpresa + " AND IDC_FECHA = " + numeroFechaSinEspacios + " AND IDC_DOCUMENTO = '" + idcDocumento + "'" +
                                     " AND IDC_CONCEPTO= 'Reemplazo POS "+numeroFactura+"'";
                    FbCommand comando = new FbCommand(cadena, fbCon);

                    fbCon.Open();
                    FbDataReader rta = comando.ExecuteReader();
                    tabla.Load(rta);
                    return tabla;
         
             
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            //string fechas = fecha.ToString("yyyy-MM-dd");
            //DateTime fechaNum = DateTime.ParseExact(fechas, "yyyy-MM-dd", null);
            //int numeroFecha = (int)(fecha - new DateTime(1899, 12, 30)).TotalDays;


        }

        public DataTable GenerarDatosASubir(int idEstacionamiento, DateTime fecha, int numeroFactura)
        {

            string fechaNew = fecha.ToString("yyyy-dd-MM");
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                SqlCommand Comando = new SqlCommand("InterfazContableNC", sqlCon);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@IdEstacionamientoConsulta", SqlDbType.VarChar).Value = idEstacionamiento;
                Comando.Parameters.Add("@FechaCosulta", SqlDbType.DateTime2).Value = fechaNew;
                Comando.Parameters.Add("@NumeroFactura", SqlDbType.VarChar).Value = numeroFactura;
                sqlCon.Open();
                SqlDataReader rta = Comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;


            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if(sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
        }

        public DataTable ListarDocumentoEmpresa(int idEstacionamiento)
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("select  Idc_Empresa, DocumentoEmpresa from T_EmpresaParquearse where IdEstacionamiento="+idEstacionamiento+" ");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;
            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }

        }

        //ListarItemsContable

        //public DataTable ListarItemsContable()
        //{
        //    FbConnection fbCon = new FbConnection();
        //    DataTable tabla = new DataTable();
        //    try
        //    {
        //        fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
        //        string cadena = ("SELECT * FROM ITEMSDOCCONTABLE");
        //        FbCommand comando = new FbCommand(cadena, fbCon);
        //        fbCon.Open();
        //        FbDataReader rta = comando.ExecuteReader();
        //        tabla.Load(rta);
        //        return tabla;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (fbCon.State == ConnectionState.Open) fbCon.Close();
        //    }



        //}


        public bool InsertarItemsContable(DataTable datos, int itemConsecutivo, int idEstacionamiento, string numeroFactura)
        {
            int consecutivoNumero = 1;
            string rta = "";
            double MyDouble = 0;
            string numero = string.Empty;
            int idc_empresa = 0;
            string documentoempresa = string.Empty;
            bool ok = true;
            string idcConcepto = "";
            int idcNumero = 0;
            DataTable tablaItems;

            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                foreach (DataRow row in datos.Rows)
                {
                    string[] nuevaFecha = row[3].ToString().Split('/');
                    DateTime MyDate = new DateTime(Convert.ToInt32(nuevaFecha[2]), Convert.ToInt32(nuevaFecha[1]), Convert.ToInt32(nuevaFecha[0]));
                    MyDouble = MyDate.ToOADate();

                    //VALIDAR SI EXISTE EL REGISTRO

                    tablaItems = ListarItemsContable();
                    if (tablaItems.Rows.Count > 0)
                    {
                        foreach (DataRow lstTabla in tablaItems.Rows)
                        {
                            idcConcepto = Convert.ToString(lstTabla["IDC_CONCEPTO"]);
                            idcNumero = Convert.ToInt32(lstTabla["IDC_NUMERO"]);
                        }

                        if (idcConcepto.ToString() != row[5].ToString())
                        {
                            consecutivoNumero = idcNumero + 1;
                        }
                        else
                        {
                            consecutivoNumero = idcNumero;
                        }
                    }

                    idc_empresa = Convert.ToInt32(row[0]);
                    documentoempresa = row[1].ToString();
                    numero = Convert.ToString(consecutivoNumero);

                    string SQLCommandText = "INSERT into ITEMSDOCCONTABLE Values ("
                                + row[0]+ ",'"+row[1]+"',"
                                + consecutivoNumero
                                + ",'"
                                + MyDouble
                                + "',"
                                + itemConsecutivo
                                + ",'"
                                + row[5]
                                + "',"
                                + row[6]
                                + ","
                                + row[7]
                                + ","
                                + row[8]
                                + ","
                                + "NULL"
                                + ",'"
                                + row[10]
                                + "',"
                                + "NULL"
                                + ","
                                + "NULL"
                                + ",'"
                                + row[13]
                                + "',"
                                + row[14]
                                + ");";

                    itemConsecutivo++;
                    fbCon.Open();
                    FbCommand comando = new FbCommand(SQLCommandText,fbCon);
                  
                    comando.ExecuteNonQuery();
                    fbCon.Close();
                    GenerarArchivoPlano(SQLCommandText);
                }
                string SQLCommandText2 = "INSERT into DOCCONTABLE Values (" + idc_empresa + ",'"
                                                                            + documentoempresa
                                                                            + "','"
                                                                            + numero
                                                                            + "','"
                                                                            + MyDouble
                                                                            + "',NULL,NULL,NULL,NULL,NULL);";
                FbCommand comando2 = new FbCommand(SQLCommandText2,fbCon);
                fbCon.Open();
                comando2.ExecuteNonQuery();
                fbCon.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return ok;
        }

        public void GenerarArchivoPlano(string texto)
        {


            // Obtener la fecha actual para el nombre de archivo
            DateTime fechaActual = DateTime.Now;
            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registro NC" + fechaActual.Day.ToString() + "-" + fechaActual.Month.ToString() + "-" + fechaActual.Year.ToString());
            string nombre = "NotaCredito";
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


    }
}

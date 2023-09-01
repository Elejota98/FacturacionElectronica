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
                DateTime fechaActual = DateTime.Now;

                DateTime fechaConsultaInicio = new DateTime(fechaActual.Year, fechaActual.Month, 1);

                DateTime fechaConsultaFin = new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month))
        .AddHours(23)
        .AddMinutes(59)
        .AddSeconds(59);

                string fechaConsultaInicioNew = Convert.ToDateTime(fechaConsultaInicio).ToString("yyyy-MM-dd HH:mm:ss");
                string fechaonsultaFinNew = Convert.ToDateTime(fechaConsultaFin).ToString("yyyy-MM-dd HH:mm:ss");
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();


                string cadena = ("SELECT MAX(T_Clientes.Identificacion) AS Identificacion, T_Estacionamientos.Nombre, TotalPayment.Total AS Total, MAX(T_PagosFE.FechaPago) AS FechaPago,  T_PagosFE.NumeroFactura,   MAX(T_PagosFE.IdModulo) AS IdModulo FROM    dbo.T_Estacionamientos " +
" INNER JOIN dbo.T_PagosFE ON dbo.T_Estacionamientos.IdEstacionamiento = dbo.T_PagosFE.IdEstacionamiento INNER JOIN dbo.T_Pagos ON dbo.T_PagosFE.NumeroFactura = dbo.T_Pagos.NumeroFactura INNER JOIN dbo.T_Clientes ON dbo.T_PagosFE.Identificacion = dbo.T_Clientes.Identificacion INNER JOIN (    SELECT " +
        " NumeroFactura,   SUM(Total) AS Total   FROM dbo.T_PagosFE    WHERE IdEstacionamiento = "+idEstacionamiento+"  AND Anulada = 0  GROUP BY NumeroFactura) AS TotalPayment ON T_PagosFE.NumeroFactura = TotalPayment.NumeroFactura WHERE   T_Pagos.FechaSolicitud BETWEEN '"+ fechaConsultaInicioNew + "' AND '"+ fechaonsultaFinNew + "'   AND T_PagosFE.IdEstacionamiento = "+idEstacionamiento+" "+
   " AND T_PagosFE.Anulada = 0 GROUP BY   T_PagosFE.NumeroFactura,  T_Estacionamientos.Nombre, TotalPayment.Total;");

                //string cadena = ("SELECT cl.Identificacion, e.Nombre, SUM(T_PagosFE.Total) AS Total, T_PagosFE.FechaPago, " +
                //    "T_PagosFE.NumeroFactura, IdModulo FROM T_PagosFE INNER JOIN T_Clientes cl on T_PagosFE.Identificacion = cl.Identificacion " +
                //    "INNER JOIN T_Estacionamientos e on T_PagosFE.IdEstacionamiento = e.IdEstacionamiento INNER JOIN T_Pagos PA ON PA.NumeroFactura = T_PagosFE.NumeroFactura" +
                //    " WHERE T_PagosFE.IdEstacionamiento=" + idEstacionamiento+ "   and PA.FechaSolicitud BETWEEN '"+fechaConsultaInicio+ "' and '"+fechaConsultaFin+ "' AND T_PagosFE.Anulada=0 GROUP BY T_PagosFE.NumeroFactura, T_PagosFE.FechaPago, cl.Identificacion, T_PagosFE.IdModulo, e.Nombre");
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

        public DataTable ListarTodosPagosInterfaz()
        {
            DateTime fechaActual = DateTime.Now;

            DateTime fechaConsultaInicio = new DateTime(fechaActual.Year, fechaActual.Month, 1);

            DateTime fechaConsultaFin = new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month))
    .AddHours(23)
    .AddMinutes(59)
    .AddSeconds(59);
            string fechaConsultaInicioNew = Convert.ToDateTime(fechaConsultaInicio).ToString("yyyy-MM-dd HH:mm:ss");
            string fechaonsultaFinNew = Convert.ToDateTime(fechaConsultaFin).ToString("yyyy-MM-dd HH:mm:ss");

            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT " +
    "  MAX(T_Clientes.Identificacion) AS Identificacion,  " +
    "  T_Estacionamientos.Nombre,   SUM(T_Pagos.Total) AS Total, " +
    "  MAX(T_PagosFE.FechaPago) AS FechaPago,   MAX(T_PagosFE.NumeroFactura) AS NumeroFactura, " +
    "   MAX(T_PagosFE.IdModulo) AS IdModulo " +
    "FROM dbo.T_Estacionamientos INNER JOIN dbo.T_PagosFE ON dbo.T_Estacionamientos.IdEstacionamiento = dbo.T_PagosFE.IdEstacionamiento " +
    "INNER JOIN dbo.T_Pagos ON dbo.T_PagosFE.NumeroFactura = dbo.T_Pagos.NumeroFactura " +
    "INNER JOIN dbo.T_Clientes ON dbo.T_PagosFE.Identificacion = dbo.T_Clientes.Identificacion WHERE  " +
    " T_Pagos.FechaSolicitud BETWEEN '"+ fechaConsultaInicioNew + "' AND '"+ fechaonsultaFinNew + "'   AND T_PagosFE.Anulada = 0 " +
    "GROUP BY    T_Estacionamientos.Nombre,   T_PagosFE.NumeroFactura;");
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
                string fechaFormateada = fechaFinal.ToString("yyyy-MM-dd");
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
                string cadena = ("SELECT i.* FROM ITEMSDOCCONTABLE i JOIN ( SELECT MAX(CAST(IDC_NUMERO AS INTEGER)) AS Max_IDC_NUMERO    FROM ITEMSDOCCONTABLE) max_value ON CAST(i.IDC_NUMERO AS INTEGER) = max_value.Max_IDC_NUMERO");
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
                DateTime MyDate = new DateTime(Convert.ToInt32(nuevaFecha[0]), Convert.ToInt32(nuevaFecha[1]), Convert.ToInt32(nuevaFecha[2]));
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

            string fechaNew = fecha.ToString("yyyy-MM-dd");
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

        public DataTable ListarPagosParaAnular(int idEstacionamiento, DateTime fecha, int numeroFactura)
        {
            DataTable tabla= new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                string fechaInicio = fecha.ToString("yyyy-MM-dd") + " 00:00:00";
                string fechaFin = fecha.ToString("yyyy-MM-dd") + " 23:59:59";
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT * FROM T_PagosFE where IdEstacionamiento=" + idEstacionamiento + " AND NumeroFactura=" + numeroFactura + " and FechaPago Between '" + fechaInicio + "' AND '" + fechaFin + "'");
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

        public string AnularFacturaPOS(int numeroFactura, int idEstacionamiento, string idModulo)
        {
            string rta = "";
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("UPDATE T_PagosFE SET Anulada=1 Where NumeroFactura=" + numeroFactura + " AND IdEstacionamiento="+idEstacionamiento+" AND IdModulo='"+idModulo+"'");
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

        public bool InsertarItemsContable(DataTable datos, int itemConsecutivo, int idEstacionamiento, string numeroFactura, string idModulo)
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
            string prefijo = string.Empty;
            DataTable tablaItems;
            DateTime FechaSolicitud = DateTime.Now;

            FbConnection fbCon = new FbConnection();
            try
            {
                tablaItems = listarPrefijoPorModulo(idModulo, idEstacionamiento);

                foreach (DataRow lstTabla in tablaItems.Rows)
                {
                     prefijo = Convert.ToString(lstTabla["Prefijo"]);
                }

                tablaItems = ConsultarFechaSolicitudFacturaElectronica(prefijo, idEstacionamiento, Convert.ToInt32(numeroFactura));


                foreach (DataRow lstTabla in tablaItems.Rows)
                {
                     FechaSolicitud = (DateTime)lstTabla["FechaSolicitud"];
                }
                DateTime fechaActual = FechaSolicitud;
                DateTime fechaSoloFecha = fechaActual.Date;
                MyDouble = fechaSoloFecha.ToOADate();
                //numero = Convert.ToString(consecutivoNumero);
                //Consecutivo
                //string numeroItem = fechaActual.ToString("ddMM");
                //consecutivoNumero = Convert.ToInt32(numeroItem);

                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                foreach (DataRow row in datos.Rows)
                {
                    //string[] nuevaFecha = row[3].ToString().Split('/');
                    //DateTime MyDate = new DateTime(Convert.ToInt32(nuevaFecha[2]), Convert.ToInt32(nuevaFecha[1]), Convert.ToInt32(nuevaFecha[0]));
                    //MyDouble = MyDate.ToOADate();



                    #region AnulaFacturaPOS



                    #endregion

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
                    idcConcepto = row[5].ToString();

                    idc_empresa = Convert.ToInt32(row[0]);
                    documentoempresa = row[1].ToString();

                    string SQLCommandText = "INSERT into ITEMSDOCCONTABLE Values ("
                                + row[0]+ ",'"+row[1]+"',"+ consecutivoNumero+ ",'"+ MyDouble + "',"+ itemConsecutivo+ ",'"+ row[5]+ "',"+ row[6]+ ","+ row[7]+ ","+ row[8]+ ","+ "NULL"+ ",'"+ row[10]+ "',"+ "NULL"+ ","+ "NULL"+ ",'"+ row[13]+ "',"+ row[14]+ ");";

                    itemConsecutivo++;
                    fbCon.Open();
                    FbCommand comando = new FbCommand(SQLCommandText,fbCon);
                  
                    comando.ExecuteNonQuery();
                    fbCon.Close();
                    GenerarArchivoPlano(SQLCommandText);
                    ok = true;
                }
                string SQLCommandText2 = "INSERT into DOCCONTABLE Values (" + idc_empresa + ",'"
                                                                            + documentoempresa
                                                                            + "','"
                                                                            + consecutivoNumero
                                                                            + "','"
                                                                            + MyDouble
                                                                            + "',NULL,NULL,0,NULL,NULL);";
                FbCommand comando2 = new FbCommand(SQLCommandText2,fbCon);
                fbCon.Open();
                comando2.ExecuteNonQuery();
                fbCon.Close();
                GenerarArchivoPlano(SQLCommandText2);
                AnularFacturaPOS(Convert.ToInt32(numeroFactura), idEstacionamiento, idModulo);
                    ok = true;
            }
            catch (Exception ex)
            {

                throw ex;
                ok=false;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
            return ok;
        }

        public DataTable listarPrefijoPorModulo(string idModulo, int idEstacionamiento)
        {
            DataTable tabla= new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNubeParking();
                string cadena = ("select Prefijo from T_Facturacion where IdModulo='" + idModulo + "' AND IdEstacionamiento=" + idEstacionamiento + "");
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


        public DataTable ConsultarFechaSolicitudFacturaElectronica(string prefijo, int idEstacionamiento, int numeroFactura)
        {
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = RepositorioConexion.getInstancia().CrearConexionNube();
                string cadena = ("SELECT FechaSolicitud FROM T_Pagos WHERE Prefijo='"+prefijo+"' AND IdEstacionamiento="+idEstacionamiento+" AND NumeroFactura='"+numeroFactura+"'");
                SqlCommand comando = new SqlCommand(cadena, sqlCon);
                sqlCon.Open();
                SqlDataReader rta = comando.ExecuteReader();
                tabla.Load(rta);
                return tabla;

            }
            catch (Exception ex)
            {

                throw ex ;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }

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

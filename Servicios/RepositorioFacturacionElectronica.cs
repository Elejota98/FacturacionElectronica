using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                string cadena = ("SELECT Identificacion,RazonSocial,Direccion,Telefono,Email, c.Nombre, Fecha,Estado FROM T_Clientes inner join T_Ciudades c on IdCiudad=c.Id where estado=0");
                SqlCommand comando = new SqlCommand(cadena, sqlcon);
                sqlcon.Open();
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
                if (sqlcon.State == ConnectionState.Open) sqlcon.Close();
            }
        }
    }
}

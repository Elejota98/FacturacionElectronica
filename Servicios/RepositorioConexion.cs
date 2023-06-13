using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class RepositorioConexion
    {
        private static RepositorioConexion con = null;
            
        public FbConnection CrearConexionlocal()
        {
            FbConnectionStringBuilder builder = new FbConnectionStringBuilder();
            builder.Database = ConfigurationManager.AppSettings["ConexionLocal"];
            FbConnection conexion = new FbConnection(builder.ToString());
            return conexion;

        }

        public SqlConnection CrearConexionNube()
        {
            SqlConnection cadena = new SqlConnection(ConfigurationManager.AppSettings["ConexionNube"]);
            return cadena;
        }

        public static RepositorioConexion getInstancia()
        {
            if(con is null)
            {
                con = new RepositorioConexion();
            }
            return con;
        }
    }
}

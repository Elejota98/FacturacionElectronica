﻿using FirebirdSql.Data.FirebirdClient;
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
            
        public FbConnection CrearConexionLocal()
        {
            FbConnection cadena = new FbConnection(ConfigurationManager.AppSettings["ConexionLocal"]);
            return cadena;

        }

        public SqlConnection CrearConexionNube()
        {
            SqlConnection cadena = new SqlConnection(ConfigurationManager.AppSettings["ConexionNube"]);
            return cadena;
        }
        public SqlConnection CrearConexionNubeParking()
        {
            SqlConnection cadena = new SqlConnection(ConfigurationManager.AppSettings["ConexionNubeParking"]);
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

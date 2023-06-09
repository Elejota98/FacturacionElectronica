using Dapper;
using FacturacionElectronica.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace FacturacionElectronica.Servicios
{
    public interface IRepositorioParametros
    {
        Task<List<Parametros>> ListarRuta();
    }
    public class RepositorioParametros : IRepositorioParametros
    {
        private readonly string connectionString;
        private readonly string connectionStringNube;
        public RepositorioParametros( IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionStringNube = configuration.GetConnectionString("ConexionNube");

        }
        

        //Traer ruta donde se va a guardar el archivo 

        public async Task<List<Parametros>> ListarRuta()
        {
            using var connection = new SqlConnection(connectionString);

            var parametros= await connection.QueryAsync<Parametros>(@"SELECT * FROM T_Parametros");
            return parametros.AsList();
        }


    }

 }


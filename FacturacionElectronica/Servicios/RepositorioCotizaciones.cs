using Dapper;
using FacturacionElectronica.Models;
using System.Data.SqlClient;

namespace FacturacionElectronica.Servicios
{
    public interface IRepositorioCotizaciones
    {
        Task<bool> ClienteExiste(int identificacion);
        Task<IEnumerable<Estacionamientos>> ListarEstacionamientos();
    }
    public class RepositorioCotizaciones : IRepositorioCotizaciones
    {
        private readonly string connectionString;
        private readonly string connectionStringNube;
        public RepositorioCotizaciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionStringNube = configuration.GetConnectionString("ConexionNube");
        }

        //Validar si el cliente existe 

        public async Task<bool> ClienteExiste(int identificacion)
        {
            using var connection = new SqlConnection(connectionString);

            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Clientes WHERE Identificacion=@Identificacion", new { identificacion });
            return existe == 1;
        }

        //Listar los estacionamientos

        public async Task<IEnumerable<Estacionamientos>> ListarEstacionamientos()
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryAsync<Estacionamientos>(@"SELECT IdEstacionamiento,Nombre FROM T_Estacionamientos WHERE Estado=1");
        }



    }
}

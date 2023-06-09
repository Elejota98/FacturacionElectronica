using Dapper;
using FacturacionElectronica.Models;
using System.Data.SqlClient;

namespace FacturacionElectronica.Servicios
{
    public interface IRepositorioPagos
    {
        Task ActualizarEstadoPago();
        Task<bool> Existe(string identificacion);
        Task Insertar(PagosCreacionViewModel pagosCreacionViewModel);
        Task<IEnumerable<Cotizaciones>> ListarDatosPagos();
        Task<IEnumerable<Estacionamientos>> ListarEstacionamientos();
        Task<Facturacion> ListarIdModuloPorPrefijo(string prefijo, int idEstacionamiento);
        Task<Pagos> ListarTotal(int numeroFactura, string idModulo, int idEstacionamiento);
        Task<IEnumerable<Facturacion>> ObtenerPrefijoPorIdEstacionamiento(long idEstacionamiento);
    }
    public class RepositorioPagos : IRepositorioPagos
    {
        private readonly string connectionString;
        private readonly string connectionStringNube;
        public RepositorioPagos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionStringNube = configuration.GetConnectionString("ConexionNube");
        }

        //Listar los estacionamientos

        public async Task<IEnumerable<Estacionamientos>> ListarEstacionamientos()
        {
            using var connection = new SqlConnection(connectionStringNube);

            return await connection.QueryAsync<Estacionamientos>("SELECT IdEstacionamiento,Nombre FROM T_Estacionamientos WHERE Estado=1");

        }
        public async Task<IEnumerable<Facturacion>> ObtenerPrefijoPorIdEstacionamiento(long idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryAsync<Facturacion>(@"SELECT IdFacturacion,Prefijo FROM T_Facturacion WHERE IdEstacionamiento=@IdEstacionamiento", new { idEstacionamiento });
        }

        //Listar modulo
        public async Task<Facturacion> ListarIdModuloPorPrefijo(string prefijo, int idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryFirstOrDefaultAsync<Facturacion>(@"SELECT IdModulo FROM T_Facturacion
                                                                            WHERE IdEstacionamiento=@IdEstacionamiento AND Prefijo=@Prefijo", new { prefijo, idEstacionamiento });
        }
        //Listar total por el idModulo 
        public async Task<Pagos> ListarTotal(int numeroFactura, string idModulo, int idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryFirstOrDefaultAsync<Pagos>(@"Select SUM(Total) as Total from T_Pagos
                                                                                                       WHERE NumeroFactura=@NumeroFactura and IdModulo=@IdModulo and IdEstacionamiento=@IdEstacionamiento", new { numeroFactura, idModulo, idEstacionamiento });
        }

        //Validar si el cliente ya existe 

        public async Task<bool> Existe(string identificacion)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Clientes WHERE Identificacion=@Identificacion", new { identificacion }); //El primer registro o un valor por defecto si no existe
           
            return existe == 1;

        }


        public async Task Insertar(PagosCreacionViewModel pagosCreacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QueryFirstOrDefaultAsync<int>("InsertarPagos", new
            {
                numeroDocumento=pagosCreacionViewModel.NumeroDocumento,
                numeroFactura=pagosCreacionViewModel.NumeroFactura,
                prefijo=pagosCreacionViewModel.Prefijo,
                total=pagosCreacionViewModel.Total,
                idEstacionamiento=pagosCreacionViewModel.IdEstacionamiento,
                imagen=pagosCreacionViewModel.Imagen

            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        //Listar datos pagos 

        public async Task<IEnumerable<Cotizaciones>> ListarDatosPagos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cotizaciones>(@"SELECT C.Empresa, c.Fecha,c.Identificacion,c.CodigoSucursal,p.Prefijo,p.NumeroFactura, c.Vendedor FROM T_Clientes C INNER JOIN 
                                                        T_Pagos P on c.Identificacion=p.NumeroDocumento  WHERE p.Estado=0");
        }

        //ACTUALIZAR ESTADO 

        public async Task ActualizarEstadoPago()
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE T_Pagos SET Estado=1 WHERE Estado=0");
        }
    }   
}

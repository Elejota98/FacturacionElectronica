using Dapper;
using FacturacionElectronica.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace FacturacionElectronica.Servicios
{
    public interface IRepositorioCliente
    {
        Task ActualizarEstado();
        Task Crear(Cliente cliente);
        Task<bool> Existe(string identificacion);
        Task<IEnumerable<Ciudades>> ListarCiudadesPorDepartamento(int id);
        Task<Ciudades> ListarCiudadesPorId(int IdCiudad);
        Task<IEnumerable<Departamentos>> ListarDepartamentos();
        Task<IEnumerable<Cliente>> ObtenerClientes();
    }

    public class RepositorioCliente : IRepositorioCliente
    {
        private readonly string connectionString;
        private readonly string connectionStringNube;
        public RepositorioCliente(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionStringNube = configuration.GetConnectionString("ConexionNube");
        }


        //Crear cliente 

        public async Task Crear(Cliente cliente)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QueryFirstOrDefaultAsync<int>("InsertarCliente", new
            {
                
                identificacion = cliente.Identificacion,
                tipoPersona =cliente.TipoPersona,
                tipoDocumento=cliente.TipoDocumento,
                nombreApellidos=cliente.NombreApellidos,
                razonSocial = cliente.RazonSocial,
                direccion = cliente.Direccion,
                telefono = cliente.Telefono,
                email = cliente.Email,
                ciudad = cliente.IdCiudad,
            }, commandType: System.Data.CommandType.StoredProcedure);

        }
        //Listar 

        public async Task<IEnumerable<Cliente>> ObtenerClientes()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cliente>(@"SELECT Identificacion,RazonSocial,Direccion,Telefono,Email, c.Nombre, Fecha,Estado FROM T_Clientes inner join T_Ciudades c on IdCiudad=c.Id where estado=0");
        }

        //Listar los departamentos 

        public async Task<IEnumerable<Departamentos>> ListarDepartamentos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Departamentos>(@"Select Id, Nombre FROM T_Departamento");
        }

        //Listar ciudades por departamentos

        public async Task<IEnumerable<Ciudades>>ListarCiudadesPorDepartamento(int IdDepartamento)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Ciudades>(@"Select Id, Nombre From T_Ciudades Where IdDepartamento=@IdDepartamento", new { IdDepartamento });
        }

        //Obtener ciudad porId

        public async Task<Ciudades> ListarCiudadesPorId(int IdCiudad)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstAsync<Ciudades>(@"SELECT Nombre FROM T_Ciudades WHERE IdCiudad=@IdCiudad", new { IdCiudad });
        }

        //Actulizar estado de la interfaz 

        public async Task ActualizarEstado()
        {
            using var connection = new SqlConnection(connectionString);
             await connection.ExecuteAsync(@"UPDATE T_Clientes SET Estado=1 WHERE Estado=0");
        }

        public async Task<bool> Existe(string identificacion)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Clientes
                                                                        WHERE Identificacion=@Identificacion", new { identificacion }); //El primer registro o un valor por defecto si no existe
            return existe == 1;

        }
    }
}

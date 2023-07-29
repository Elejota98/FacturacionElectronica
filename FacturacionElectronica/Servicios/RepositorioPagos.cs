using Dapper;
using FacturacionElectronica.Models;
using System.Data.SqlClient;

namespace FacturacionElectronica.Servicios
{
    public interface IRepositorioPagos
    {
        Task ActualizarEstadoPago();
        Task<bool> Existe(string identificacion);
        Task<bool> ExisteFactura(int idEstacionamiento, string idModulo, DateTime fechaPago, int numeroFactura);
        Task<bool> ExisteFacturaElectronica(int idEstacionamiento, string prefijo, DateTime fechaPago, int numeroFactura);
        Task Insertar(PagosCreacionViewModel pagosCreacionViewModel);
        Task InsertarPagosFE(PagosNube pagosNube);
        Task<IEnumerable<Estacionamientos>> ListarEstacionamientos();
        Task<List<ReporteFacturas>> ListarFacturas(string fechaInicio, string fechaFinal);
        Task<Facturacion> ListarIdModuloPorPrefijo(string prefijo, int idEstacionamiento);
        Task<List<PagosNube>> ListarPagos(int idPago);
        Task<List<PagosNube>> ListarPagosNube(int numeroFactura, int idEstacionamiento, string idModulo);
        Task<TipoPagos> ListarTipoPago(string tipoPago);
        Task<List<Tarifas>> ListarTipoVehiculo(long IdAutorizacion, long IdTipoPago, long IdEstacionamiento);
        Task<Pagos> ListarTotal(int numeroFactura, string idModulo, int idEstacionamiento);
        Task<List<Pagos>> ListarTotalesSeparados(int numeroFactura, string idModulo, int idEstacionamiento);
        Task<IEnumerable<Facturacion>> ObtenerPrefijoPorIdEstacionamiento(long idEstacionamiento);
        Task<bool> VerificarClienteExiste(int identificacion);
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

            return await connection.QueryAsync<Estacionamientos>("SELECT IdEstacionamiento,Nombre FROM T_Estacionamientos WHERE Estado=1 Order By Nombre");

        }
        public async Task<IEnumerable<Facturacion>> ObtenerPrefijoPorIdEstacionamiento(long idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryAsync<Facturacion>(@"SELECT IdFacturacion,Prefijo FROM T_Facturacion WHERE IdEstacionamiento=@IdEstacionamiento AND Estado=1", new { idEstacionamiento });
        }

        //Listar modulo
        public async Task<Facturacion> ListarIdModuloPorPrefijo(string prefijo, int idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryFirstOrDefaultAsync<Facturacion>(@"SELECT IdModulo FROM T_Facturacion
                                                                            WHERE IdEstacionamiento=@IdEstacionamiento AND Prefijo=@Prefijo AND Estado=1", new { prefijo, idEstacionamiento });
        }
        //Listar total por el idModulo 
        public async Task<Pagos> ListarTotal(int numeroFactura, string idModulo, int idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            return await connection.QueryFirstOrDefaultAsync<Pagos>(@"Select SUM(Total) as Total from T_Pagos
                                                                     WHERE NumeroFactura=@NumeroFactura and IdModulo=@IdModulo and IdEstacionamiento=@IdEstacionamiento", new { numeroFactura, idModulo, idEstacionamiento });
        }

        //Listar total para insertar a la bd 

        public async Task<List<Pagos>> ListarTotalesSeparados(int numeroFactura, string idModulo, int idEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            var resultado = await connection.QueryAsync<Pagos>(@"Select IdPago, NumeroFactura,Total, IdTipoPago from T_Pagos
                                                                     WHERE NumeroFactura=@NumeroFactura and IdModulo=@IdModulo and IdEstacionamiento=@IdEstacionamiento", new { numeroFactura, idModulo, idEstacionamiento });

            return resultado.ToList();
        }

        //Validar si el cliente ya existe 

        public async Task<bool> Existe(string identificacion)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Clientes WHERE Identificacion=@Identificacion", new { identificacion }); //El primer registro o un valor por defecto si no existe
           
            return existe == 1;

        }

        public async Task<bool> VerificarClienteExiste(int identificacion)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Clientes WHERE Identificacion=@Identificacion AND Estado=0", new { identificacion });
            return existe == 1;

        }

        //Verificar si existe la factura 

        public async Task<bool>ExisteFactura(int idEstacionamiento, string idModulo, DateTime fechaPago, int numeroFactura)
        {
            string FechaInicio = fechaPago.ToString("yyyy-MM-dd") + " 00:00:00";
            string FechaFin = fechaPago.ToString("yyyy-MM-dd") + " 23:59:59";


            using var connection = new SqlConnection(connectionStringNube);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Pagos
                                                                            WHERE IdEstacionamiento=@IdEstacionamiento and IdModulo=@IdModulo and NumeroFactura=@NumeroFactura and FechaPago  Between @FechaInicio and @FechaFin",
                                                                            new {idEstacionamiento,idModulo,numeroFactura,FechaInicio,FechaFin});
            return existe == 1;
        }

        public async Task<bool> ExisteFacturaElectronica(int idEstacionamiento, string prefijo, DateTime fechaPago, int numeroFactura)
        {
            string FechaInicio = fechaPago.ToString("yyyy-MM-dd") + " 00:00:00";
            string FechaFin = fechaPago.ToString("yyyy-MM-dd") + " 23:59:59";


            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM T_Pagos
                                                                            WHERE IdEstacionamiento=@IdEstacionamiento and Prefijo=@Prefijo and NumeroFactura=@NumeroFactura and FechaPago  Between @FechaInicio and @FechaFin",
                                                                            new { idEstacionamiento, prefijo, numeroFactura, FechaInicio, FechaFin });
            return existe == 1;
        }

        public async Task<List<PagosNube>> ListarPagosNube(int numeroFactura, int idEstacionamiento, string idModulo)
        {
            using var connection = new SqlConnection(connectionStringNube);
            var rta = await connection.QueryAsync<PagosNube>(@"select * from T_Pagos
                                                                        where IdModulo=@IdModulo and NumeroFactura=@NumeroFactura and IdEstacionamiento=@IdEstacionamiento ORDER BY 1 DESC",
                                                                        new { idModulo, numeroFactura, idEstacionamiento });

        return rta.ToList();
        }




        public async Task Insertar(PagosCreacionViewModel pagosCreacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QueryFirstOrDefaultAsync<int>("InsertarPagos", new
            {
                numeroDocumento=pagosCreacionViewModel.Identificacion,
                numeroFactura=pagosCreacionViewModel.NumeroFactura,
                prefijo=pagosCreacionViewModel.Prefijo,
                total=pagosCreacionViewModel.Total,
                idEstacionamiento=pagosCreacionViewModel.IdEstacionamiento,
                idTipoPago = pagosCreacionViewModel.IdTipoPago,
                fechaPago = pagosCreacionViewModel.FechaPago,
                imagen=pagosCreacionViewModel.Imagen

            }, commandType: System.Data.CommandType.StoredProcedure);

            pagosCreacionViewModel.IdPago = id;
        }

        //ACTUALIZAR ESTADO 

        public async Task ActualizarEstadoPago()
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE T_Pagos SET Estado=1 WHERE Estado=0");
        }

        //LISTAT TIPO PAGO 

        public async Task<TipoPagos> ListarTipoPago(string tipoPago)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoPagos>(@"select IdTipopago from T_TipoPago WHERE TipoPago=@TipoPago", new { tipoPago });
        }

        #region REPORTES

        public async Task<List<ReporteFacturas>> ListarFacturas(string fechaInicio, string fechaFinal)
        {
            using var connection = new SqlConnection(connectionString);


            var listado = await connection.QueryAsync<ReporteFacturas>("ListarFacturasElectronicas",
                new
                {
                    fechainicio = fechaInicio,
                    fechaFinal = fechaFinal
                }, commandType: System.Data.CommandType.StoredProcedure);

            return listado.ToList();
        }

        public async Task<List<PagosNube>> ListarPagos(int idPago)
        {
            using var connection = new SqlConnection(connectionStringNube);
            var listado = await connection.QueryAsync<PagosNube>(@"SELECT * FROM T_Pagos WHERE IdPago=@IdPago", new { IdPago = idPago });
            return listado.ToList();
        }

        public async Task InsertarPagosFE(PagosNube pagosNube)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Insert into T_PagosFE (IdPago, Identificacion, IdAutorizado, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura, Sincronizacion, PagoMensual,Anulada)
                                    VALUES (@IdPago, @IdTransaccion, @IdAutorizado, @IdEstacionamiento, @IdModulo, @IdFacturacion, @IdTipoPago, @FechaPago, @Subtotal, @Iva, @Total, @NumeroFactura,@Sincronizacion,@PagoMensual, @Anulada)",
                                            new
                                            {
                                                pagosNube.IdPago,
                                                pagosNube.IdTransaccion,
                                                pagosNube.IdAutorizado,
                                                pagosNube.IdEstacionamiento,
                                                pagosNube.IdModulo,
                                                pagosNube.IdFacturacion,
                                                pagosNube.IdTipoPago,
                                                pagosNube.FechaPago,
                                                pagosNube.Subtotal,
                                                pagosNube.Iva,
                                                pagosNube.Total,
                                                pagosNube.NumeroFactura,
                                                pagosNube.Sincronizacion,
                                                pagosNube.PagoMensual,
                                                pagosNube.Anulada
                                            });
        }

        public async Task<List<Tarifas>> ListarTipoVehiculo(long IdAutorizacion, long IdTipoPago, long IdEstacionamiento)
        {
            using var connection = new SqlConnection(connectionStringNube);
            var listado = await connection.QueryAsync<Tarifas>("select IdTipoVehiculo from T_Tarifas where IdAutorizacion=@IdAutorizacion and IdTipoPago=@IdTipoPago" +
                                                                "    and IdEstacionamiento=@IdEstacionamiento", new { IdAutorizacion, IdTipoPago, IdEstacionamiento });
            return listado.ToList();    
        }

        #endregion
    }
}

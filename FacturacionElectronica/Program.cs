using FacturacionElectronica.Servicios;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioCliente, RepositorioCliente>();
builder.Services.AddTransient<IRepositorioParametros, RepositorioParametros>();
builder.Services.AddTransient<IRepositorioCotizaciones, RepositorioCotizaciones>();
builder.Services.AddTransient<IRepositorioPagos, RepositorioPagos>();

var app = builder.Build();

// Configurar la canalizaci�n de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 d�as. Puedes cambiarlo para escenarios de producci�n, consulta https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pagos}/{action=Index}/{id?}");

app.Run();
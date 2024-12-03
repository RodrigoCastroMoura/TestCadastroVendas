using Microsoft.EntityFrameworkCore;

using TestCadastroVendas.Api;
using TestCadastroVendas.Api.Infra.Configurations;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Infra.Mappers.TestCadastroVendasProfile;
using TestCadastroVendas.Infra.Persistence.Sql.Contexts;
using TestCadastroVendas.Infra.Persistence.Sql.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(VendasProfile));

var app = builder.Build();

app.UseCustomSwagger();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
  
    endpoints.MapControllers();
});

// Garantir que o banco de dados esteja criado ao iniciar o aplicativo
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}


await app.RunAsync();

public partial class Program { }

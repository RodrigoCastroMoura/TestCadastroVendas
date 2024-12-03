using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Infra.Persistence.Sql.Contexts;
using TestCadastroVendas.Infra.Persistence.Sql.Repositories;
using Xunit;

namespace TestCadastroVendas.Test.Unit.Infra.Persistence;

[TestClass]
public class DataContextTests : IAsyncLifetime
{
    private DataContext context;
    private IRepository<Venda> vendaRepository;
    private readonly Faker faker;
    private Venda venda;

    public DataContextTests()
    {
        
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite("Data Source=mydatabase.db;") 
            .Options;

        context = new DataContext(options);
        vendaRepository = new Repository<Venda>(context);
        faker = new Faker();
        venda = new Venda
{

            Cliente = faker.Name.FullName(),
            ValorTotalVenda = 100m,
            DataVenda = System.DateTime.Now,
            Cancelado = false,
            Filial = "Filial A",
        };
    }

    public async Task InitializeAsync()
    {
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();
    }


    public async Task DisposeAsync()
    {
        await context.Database.CloseConnectionAsync();
        context.Dispose();
    }

    [TestMethod]
    public async Task SHOULD_INTEGRATION_DATABASE()
    {
        // Arrange
        await InitializeAsync();
        // Act
        await vendaRepository.AddAsync(venda);
       

        // Assert
        var vendasDoBanco = await vendaRepository.GetByIdAsync(venda.NumeroVenda);
        await DisposeAsync();

        Xunit.Assert.NotNull(vendasDoBanco); 
        Xunit.Assert.Equal("Filial A", venda.Filial);
        Xunit.Assert.Equal(100.0m, venda.ValorTotalVenda);
    }
}


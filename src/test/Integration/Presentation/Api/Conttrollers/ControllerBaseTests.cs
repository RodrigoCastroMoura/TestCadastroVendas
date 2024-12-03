using TestCadastroVendas.Infra.Persistence.Sql.Contexts;
using TestCadastroVendas.Test.Integration.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace TestCadastroVendas.Test.Integration.Presentation.Api.Conttrollers;

public abstract class ControllerBaseTests : InfraBaseTests
{
    protected WebApplicationFactory<Program> WebAppFactory { get; private set; }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        WebAppFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(cfg => cfg
                .ConfigureServices(services => services
                    .RemoveAll<IMongoDatabase>()
                    .RemoveAll<DbContextOptions<DataContext>>()
                    .RemoveAll<DataContext>()
                    .AddScoped(_ => MongoDatabase)
                    .AddDbContext<DataContext>(x => x
                        .UseInMemoryDatabase("TestsDb")
                    )
                )
            );
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        base.TestCleanup();

        WebAppFactory.Dispose();
    }
}

using AutoMapper;
using TestCadastroVendas.Infra.Mappers.TestCadastroVendasProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mongo2Go;
using MongoDB.Driver;

namespace TestCadastroVendas.Test.Integration.Shared;

public abstract class InfraBaseTests
{
    private MongoDbRunner _runner;

    protected IMapper Mapper { get; private set; }

    protected IMongoDatabase MongoDatabase { get; private set; }

    [TestInitialize]
    public virtual void TestInitialize()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile<TestCadastroVendasProfile>();
        });

        Mapper = config.CreateMapper();

        _runner = MongoDbRunner.Start(singleNodeReplSet: true);

        var mongoClient = new MongoClient(_runner.ConnectionString);
        MongoDatabase = mongoClient.GetDatabase("TestsDB");
    }

    [TestCleanup]
    public virtual void TestCleanup() =>
        _runner.Dispose();
}

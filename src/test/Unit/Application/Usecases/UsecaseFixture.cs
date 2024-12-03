using AutoMapper;
using TestCadastroVendas.Infra.Mappers.TestCadastroVendasProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCadastroVendas.Test.Unit.Application.Usecases;

public abstract class UsecaseFixture
{
    protected IMapper _mapper;

    [TestInitialize]
    public virtual void TestInitialize()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile<TestCadastroVendasProfile>();
        });

        _mapper = config.CreateMapper();
    }
}

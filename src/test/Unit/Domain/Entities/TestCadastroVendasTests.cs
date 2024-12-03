using TestCadastroVendas.Test.Shared.Dto;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCadastroVendas.Test.Unit.Domain.Entities;

[TestClass]
public class TestCadastroVendasDomainTests
{
    [TestMethod]
    public void SHOULD_CREATE_BOILERPLATE()
    {
        var testCadastroVendasDTO = CreateTestCadastroVendasDefaultTestDto.GetDefault();
        var testCadastroVendas = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDTO.Name, testCadastroVendasDTO.TestCadastroVendasType);

        testCadastroVendas.Name.Should().Be(testCadastroVendasDTO.Name);
        testCadastroVendas.TestCadastroVendasType.Should().Be(testCadastroVendasDTO.TestCadastroVendasType);
    }


    [TestMethod]
    public void SHOULD_CREATE_BOILERPLATE_WITH_EMPTY_CONSTRUCTOR()
    {
        var id = Guid.NewGuid().ToString();
        var testCadastroVendas = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(id);

        testCadastroVendas.Id.Should().Be(id);
        testCadastroVendas.OwnerId.Should().Be(null);
    }
}

using TestCadastroVendas.Dto.TestCadastroVendas;
using TestCadastroVendas.Infra.Persistence.MongoDb.Repositories;
using TestCadastroVendas.Test.Integration.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCadastroVendas.Test.Integration.Infra.Persistence.MongoDb.Repositories;

[TestClass]
public class TestCadastroVendasProjectionRepositoryTests : InfraBaseTests
{
    [TestMethod]
    public async Task SHOULD_UPDATE_BOILERPLATE()
    {
        #region Arrange
        var newTestCadastroVendasDto = new TestCadastroVendasCreateDto("Repository Update Test - Created", CrossCutting.Enums.TestCadastroVendasType.AWS);
        var newTestCadastroVendas = Domain.Entities.TestCadastroVendas.Create(newTestCadastroVendasDto.Name, newTestCadastroVendasDto.TestCadastroVendasType);

        var testCadastroVendasRepositoryMongoDb = new TestCadastroVendasProjectionRepository(MongoDatabase);

        await testCadastroVendasRepositoryMongoDb.Insert(newTestCadastroVendas, CancellationToken.None);
        #endregion

        #region Act
        var updateTestCadastroVendasName = "Repository Update Test - Updated";
        var updateTestCadastroVendasDto = new TestCadastroVendasCreateDto(updateTestCadastroVendasName, newTestCadastroVendas.TestCadastroVendasType);
        var updateTestCadastroVendas = Domain.Entities.TestCadastroVendas.Create(updateTestCadastroVendasDto.Name, updateTestCadastroVendasDto.TestCadastroVendasType);
        updateTestCadastroVendas.Id = newTestCadastroVendas.Id;

        await testCadastroVendasRepositoryMongoDb.Update(updateTestCadastroVendas, CancellationToken.None);
        #endregion

        #region Assert
        var checkTestCadastroVendas = await testCadastroVendasRepositoryMongoDb.GetById(updateTestCadastroVendas.Id);

        updateTestCadastroVendas.Id.Should().Be(checkTestCadastroVendas.Id);
        updateTestCadastroVendas.Name.Should().Be(checkTestCadastroVendas.Name);
        updateTestCadastroVendas.TestCadastroVendasType.Should().Be(checkTestCadastroVendas.TestCadastroVendasType); 
        #endregion
    }

    [TestMethod]
    public async Task SHOULD_NOT_GET_BOILERPLATE()
    {
        #region Arrange
        var testCadastroVendasRepositoryMongoDb = new TestCadastroVendasProjectionRepository(MongoDatabase);
        #endregion

        #region Act
        var testCadastroVendas = await testCadastroVendasRepositoryMongoDb.GetById("Id doesn´t exists");
        #endregion

        #region Assert
        testCadastroVendas.Should().BeNull(); 
        #endregion
    }
}

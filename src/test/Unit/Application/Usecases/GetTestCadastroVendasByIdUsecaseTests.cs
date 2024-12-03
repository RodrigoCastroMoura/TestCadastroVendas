using TestCadastroVendas.Application.Usecases.GetTestCadastroVendasById;
using TestCadastroVendas.Domain.Repositories.MongoDb;
using TestCadastroVendas.Dto.TestCadastroVendas;
using ErrorOr;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestCadastroVendas.Test.Unit.Application.Usecases;

[TestClass]
public class GetTestCadastroVendasByIdUsecaseTests : UsecaseFixture
{        
    [TestMethod]
    public async Task SHOULD_GET_BOILERPLATE()
    {
        #region Arrange
        var testCadastroVendasDto = new TestCadastroVendasCreateDto("Test Name", CrossCutting.Enums.TestCadastroVendasType.Azure);
        var testCadastroVendasEntity = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDto.Name, testCadastroVendasDto.TestCadastroVendasType);
        testCadastroVendasEntity.Id = Guid.NewGuid().ToString();

        var testCadastroVendasRepositoryMongoDB = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasRepositoryMongoDB.Setup(x => x.GetById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(testCadastroVendasEntity);

        var getTestCadastroVendasByIdUsecase = new GetTestCadastroVendasByIdUsecase(_mapper, testCadastroVendasRepositoryMongoDB.Object);
        #endregion

        #region Act
        var testCadastroVendasDtoResult = await getTestCadastroVendasByIdUsecase.Execute(TestCadastroVendasGetByIdFilterDto.From(testCadastroVendasEntity.Id), default);
        #endregion

        #region Assert
        testCadastroVendasDtoResult.Should().NotBeNull();
        testCadastroVendasDtoResult.Should().BeOfType<ErrorOr<TestCadastroVendasDto>>();
        testCadastroVendasDto.Name.Should().Be(testCadastroVendasDtoResult.Value.Name);
        #endregion

    }

    [TestMethod]
    public async Task SHOULD_BOILERPLATE_NOT_FOUND()
    {
        #region Arrange
        var testCadastroVendasRepositoryMongoDB = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasRepositoryMongoDB.Setup(x => x.GetById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var getTestCadastroVendasByIdUsecase = new GetTestCadastroVendasByIdUsecase(_mapper, testCadastroVendasRepositoryMongoDB.Object);
        #endregion

        #region Act
        var testCadastroVendas = await getTestCadastroVendasByIdUsecase.Execute(TestCadastroVendasGetByIdFilterDto.From("Id doesn´t exists"), default);
        #endregion

        #region Assert
        testCadastroVendas.IsError.Should().BeTrue();
        testCadastroVendas.FirstError.Should().Match<Error>(x => x.Description == "TestCadastroVendas não encontrado");
        #endregion
    }
}

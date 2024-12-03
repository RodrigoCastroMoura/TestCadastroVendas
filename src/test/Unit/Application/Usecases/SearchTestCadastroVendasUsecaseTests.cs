using TestCadastroVendas.Application.Usecases.SearchTestCadastroVendas;
using TestCadastroVendas.Domain.Repositories.MongoDb;
using TestCadastroVendas.Dto.TestCadastroVendas;
using Common.Core.Dto.Search;
using ErrorOr;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestCadastroVendas.Test.Unit.Application.Usecases;

[TestClass]
public class SearchTestCadastroVendasUsecaseTests : UsecaseFixture
{
    [TestMethod]
    public async Task SHOULD_SEARCH_BOILERPLATES()
    {
        #region Arrange
        var testCadastroVendasDto = new TestCadastroVendasCreateDto("Test Name", CrossCutting.Enums.TestCadastroVendasType.Azure);
        var testCadastroVendasEntity = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDto.Name, testCadastroVendasDto.TestCadastroVendasType);
        testCadastroVendasEntity.Id = Guid.NewGuid().ToString();
        var testCadastroVendasList = new List<TestCadastroVendas.Domain.Entities.TestCadastroVendas> { testCadastroVendasEntity };

        var testCadastroVendasRepositoryMongoDB = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasRepositoryMongoDB.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((testCadastroVendasList.Count, testCadastroVendasList));

        var listTestCadastroVendasUsecase = new SearchTestCadastroVendasUsecase(_mapper, testCadastroVendasRepositoryMongoDB.Object);
        #endregion

        #region Act
        var testCadastroVendassResult = await listTestCadastroVendasUsecase.Execute(new TestCadastroVendasSearchFilterDto(), default);
        #endregion

        #region Assert
        testCadastroVendassResult.Should().NotBeNull();
        testCadastroVendassResult.Should().BeOfType<ErrorOr<PagedResultDto<TestCadastroVendasDto>>>();
        testCadastroVendasList.LongCount().Should().Be(testCadastroVendassResult.Value.Total);
        testCadastroVendasList.Count.Should().Be(testCadastroVendassResult.Value.Count);
        testCadastroVendasDto.Name.Should().Be(testCadastroVendassResult.Value.Items.First().Name);
        #endregion
    }
}

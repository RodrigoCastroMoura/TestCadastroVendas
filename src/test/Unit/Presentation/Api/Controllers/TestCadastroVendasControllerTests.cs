using TestCadastroVendas.Api.Controllers.v1;
using TestCadastroVendas.Application.Usecases.CreateTestCadastroVendas;
using TestCadastroVendas.Application.Usecases.GetTestCadastroVendasById;
using TestCadastroVendas.Application.Usecases.SearchTestCadastroVendas;
using TestCadastroVendas.Dto.TestCadastroVendas;
using Common.Core.Dto.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestCadastroVendas.Test.Unit.Presentation.Api.Controllers;

[TestClass]
public class TestCadastroVendasControllerTests
{
    [TestMethod]
    public async Task SHOULD_CREATE_BOILERPLATE()
    {
        #region arrange
        var nameResponse = "Create Async Test";
        var testCadastroVendasResponseDto = new TestCadastroVendasDto(Guid.NewGuid().ToString(), nameResponse, CrossCutting.Enums.TestCadastroVendasType.Azure);
        var createTestCadastroVendasUsecaseMock = new Mock<ICreateTestCadastroVendasUsecase>();
        createTestCadastroVendasUsecaseMock
            .Setup(x => x.Execute(It.IsAny<TestCadastroVendasCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testCadastroVendasResponseDto);

        var testCadastroVendasController = new TestCadastroVendasController(createTestCadastroVendasUsecaseMock.Object, default, default);
        #endregion

        #region act
        var result = await testCadastroVendasController.Create(new TestCadastroVendasCreateDto(nameResponse, CrossCutting.Enums.TestCadastroVendasType.Azure), default);
        #endregion

        #region assert
        result.Should().NotBeNull();
        var createdResult = result.Should().BeOfType<ActionResult<TestCadastroVendasDto>>().Subject;
        var testCadastroVendasResult = (createdResult.Result as ObjectResult).Value.Should().BeAssignableTo<TestCadastroVendasDto>().Subject;
        testCadastroVendasResult.Id.Should().Be(testCadastroVendasResponseDto.Id);
        #endregion
    }

    [TestMethod]
    public async Task SHOULD_LIST_BOILERPLATES()
    {
        #region arrange
        var nameOfTestCadastroVendas = "TEST SHOULD_GET_BOILERPLATES";

        var testCadastroVendasResponseDto = new TestCadastroVendasDto(Guid.NewGuid().ToString(), nameOfTestCadastroVendas, CrossCutting.Enums.TestCadastroVendasType.Azure);
        var listOfTestCadastroVendassDto = new List<TestCadastroVendasDto> { testCadastroVendasResponseDto };
        var testCadastroVendasGetResponse = new PagedResultDto<TestCadastroVendasDto>(listOfTestCadastroVendassDto.Count, listOfTestCadastroVendassDto);

        var listTestCadastroVendasUsecaseMock = new Mock<ISearchTestCadastroVendasUsecase>();
        listTestCadastroVendasUsecaseMock
            .Setup(x => x.Execute(It.IsAny<TestCadastroVendasSearchFilterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testCadastroVendasGetResponse);

        var testCadastroVendasController = new TestCadastroVendasController(default, listTestCadastroVendasUsecaseMock.Object, default);
        #endregion

        #region act
        var result = await testCadastroVendasController.Search(new TestCadastroVendasSearchFilterDto { Offset = 0, Limit = 10 }, default);
        #endregion

        #region assert
        result.Should().NotBeNull();
        var objectResult = result.Should().BeOfType<ActionResult<PagedResultDto<TestCadastroVendasDto>>>().Subject;
        var testCadastroVendasResult = (objectResult.Result as ObjectResult).Value.Should().BeAssignableTo<PagedResultDto<TestCadastroVendasDto>>().Subject;
        testCadastroVendasResult.Total.Should().Be(listOfTestCadastroVendassDto.Count);
        testCadastroVendasResult.Items.First().Name.Should().Be(testCadastroVendasResponseDto.Name);
        #endregion
    }

    [TestMethod]
    public async Task SHOULD_GET_BOILERPLATE_BY_ID()
    {
        #region arrange
        var nameOfTestCadastroVendas = "TEST SHOULD_GET_BOILERPLATE_BY_ID";
        var testCadastroVendasDtoResponse = new TestCadastroVendasDto(Guid.NewGuid().ToString(), nameOfTestCadastroVendas, CrossCutting.Enums.TestCadastroVendasType.Azure);

        var getTestCadastroVendasByIdUsecaseMock = new Mock<IGetTestCadastroVendasByIdUsecase>();
        getTestCadastroVendasByIdUsecaseMock
            .Setup(x => x.Execute(It.IsAny<TestCadastroVendasGetByIdFilterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testCadastroVendasDtoResponse);

        var testCadastroVendasController = new TestCadastroVendasController(default, default, getTestCadastroVendasByIdUsecaseMock.Object);
        #endregion

        #region act
        var result = await testCadastroVendasController.GetById(Guid.NewGuid().ToString(), default);
        #endregion

        #region assert
        result.Should().NotBeNull();
        var okResult = result.Should().BeOfType<ActionResult<TestCadastroVendasDto>>().Subject;
        var testCadastroVendasResult = (okResult.Result as ObjectResult).Value.Should().BeAssignableTo<TestCadastroVendasDto>().Subject;
        testCadastroVendasResult.Name.Should().Be(nameOfTestCadastroVendas);
        #endregion

    }
}

using TestCadastroVendas.Application.ExternalServices;
using TestCadastroVendas.Application.Usecases.CreateTestCadastroVendas;
using TestCadastroVendas.Domain.Repositories.MongoDb;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Dto.TestCadastroVendas;
using ErrorOr;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestCadastroVendas.Test.Unit.Application.Usecases;

[TestClass]
public class CreateTestCadastroVendasUsecaseTests : UsecaseFixture
{
    [TestMethod]
    [DataRow("Test Name")]
    [DataRow("Other Name")]
    public async Task SHOULD_CREATE_BOILERPLATE(string name)
    {
        #region Arrange
        var messageBroker = new Mock<IMessageBroker>();
        var eventStreaming = new Mock<IEventStreaming>();

        var testCadastroVendasDto = new TestCadastroVendasCreateDto(name, CrossCutting.Enums.TestCadastroVendasType.Azure);
        var testCadastroVendasEntity = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDto.Name, testCadastroVendasDto.TestCadastroVendasType);
        testCadastroVendasEntity.Id = Guid.NewGuid().ToString();

        var testCadastroVendasProjectionRepository = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasProjectionRepository.Setup(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()))
            .Callback<TestCadastroVendas.Domain.Entities.TestCadastroVendas, CancellationToken>((testCadastroVendas, _) =>
            {
                testCadastroVendas.Id = testCadastroVendasEntity.Id;
            });

        var testCadastroVendasRepository = new Mock<ITestCadastroVendasRepository>();
        testCadastroVendasRepository.Setup(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()))
            .Callback<TestCadastroVendas.Domain.Entities.TestCadastroVendas, CancellationToken>((testCadastroVendas, _) =>
            {
                testCadastroVendas.Id = testCadastroVendasEntity.Id;
            });

        var createTestCadastroVendasUsecase = new CreateTestCadastroVendasUsecase(
            _mapper, messageBroker.Object, eventStreaming.Object,
            testCadastroVendasProjectionRepository.Object, testCadastroVendasRepository.Object
        );
        #endregion

        #region Act
        var testCadastroVendasId = await createTestCadastroVendasUsecase.Execute(testCadastroVendasDto, default);
        #endregion

        #region Assert
        testCadastroVendasId.Should().NotBeNull();
        testCadastroVendasId.Should().BeOfType<ErrorOr<TestCadastroVendasDto>>();
        testCadastroVendasId.Value.Id.Should().Be(testCadastroVendasEntity.Id);
        testCadastroVendasEntity.Name.Should().Be(testCadastroVendasDto.Name);
        testCadastroVendasEntity.TestCadastroVendasType.Should().Be(testCadastroVendasDto.TestCadastroVendasType);

        messageBroker.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Once);
        messageBroker.Verify(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Once);
        eventStreaming.Verify(x => x.SendEvent(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        testCadastroVendasProjectionRepository.Verify(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()), Times.Once());
        testCadastroVendasRepository.Verify(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()), Times.Once());
        #endregion
    }

    [TestMethod]
    [DataRow(null)]
    public async Task SHOULD_NOT_CREATE_BOILERPLATE_WITH_NULL_NAME(string name)
    {
        #region Arrange
        var messageBroker = new Mock<IMessageBroker>();
        var eventStreaming = new Mock<IEventStreaming>();

        var testCadastroVendasDto = new TestCadastroVendasCreateDto(name, CrossCutting.Enums.TestCadastroVendasType.Azure);
        var testCadastroVendasEntity = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDto.Name, testCadastroVendasDto.TestCadastroVendasType);
        testCadastroVendasEntity.Id = Guid.NewGuid().ToString();

        var testCadastroVendasProjectionRepository = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasProjectionRepository.Setup(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()))
            .Callback<TestCadastroVendas.Domain.Entities.TestCadastroVendas, CancellationToken>((testCadastroVendas, _) =>
            {
                testCadastroVendas.Id = testCadastroVendasEntity.Id;
            });

        var createTestCadastroVendasUsecase = new CreateTestCadastroVendasUsecase(
           _mapper, messageBroker.Object, eventStreaming.Object, testCadastroVendasProjectionRepository.Object, null
        );
        #endregion

        #region Act
        var testCadastroVendasId = await createTestCadastroVendasUsecase.Execute(testCadastroVendasDto, default);
        #endregion

        #region Assert
        testCadastroVendasId.Should().NotBeNull();
        testCadastroVendasId.Should().BeOfType<ErrorOr<TestCadastroVendasDto>>();
        testCadastroVendasId.IsError.Should().BeTrue();
        testCadastroVendasId.Errors.Count.Should().Be(1);
        testCadastroVendasId.FirstError.Should().Match<Error>(x => x.Description == "'Nome do TestCadastroVendas' must not be empty.");

        messageBroker.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Never);
        messageBroker.Verify(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Never);
        eventStreaming.Verify(x => x.SendEvent(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        #endregion
    }

    [TestMethod]
    [DataRow("a")]
    [DataRow("ab")]
    public async Task SHOULD_NOT_CREATE_BOILERPLATE_WITH_INVALID_NAME(string name)
    {
        #region Arrange
        var messageBroker = new Mock<IMessageBroker>();
        var eventStreaming = new Mock<IEventStreaming>();

        var testCadastroVendasDto = new TestCadastroVendasCreateDto(name, CrossCutting.Enums.TestCadastroVendasType.Azure);
        var testCadastroVendasEntity = TestCadastroVendas.Domain.Entities.TestCadastroVendas.Create(testCadastroVendasDto.Name, testCadastroVendasDto.TestCadastroVendasType);
        testCadastroVendasEntity.Id = Guid.NewGuid().ToString();

        var testCadastroVendasProjectionRepository = new Mock<ITestCadastroVendasProjectionRepository>();
        testCadastroVendasProjectionRepository.Setup(x => x.Insert(It.IsAny<TestCadastroVendas.Domain.Entities.TestCadastroVendas>(), It.IsAny<CancellationToken>()))
            .Callback<TestCadastroVendas.Domain.Entities.TestCadastroVendas, CancellationToken>((testCadastroVendas, _) =>
            {
                testCadastroVendas.Id = testCadastroVendasEntity.Id;
            });

        var createTestCadastroVendasUsecase = new CreateTestCadastroVendasUsecase(
           _mapper, messageBroker.Object, eventStreaming.Object, testCadastroVendasProjectionRepository.Object, null
        );
        #endregion

        #region Act
        var testCadastroVendasId = await createTestCadastroVendasUsecase.Execute(testCadastroVendasDto, default);
        #endregion

        #region Assert
        testCadastroVendasId.Should().NotBeNull();
        testCadastroVendasId.Should().BeOfType<ErrorOr<TestCadastroVendasDto>>();
        testCadastroVendasId.IsError.Should().BeTrue();
        testCadastroVendasId.Errors.Count.Should().Be(1);
        testCadastroVendasId.FirstError.Should().Match<Error>(x => x.Description.Contains("'Nome do TestCadastroVendas' must be between 3 and 100 characters"));

        messageBroker.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Never);
        messageBroker.Verify(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<CancellationToken>()), Times.Never);
        eventStreaming.Verify(x => x.SendEvent(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        #endregion
    }
}

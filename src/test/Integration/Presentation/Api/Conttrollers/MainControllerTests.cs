using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCadastroVendas.Test.Integration.Presentation.Api.Conttrollers;

[TestClass]
public class MainControllerTests : ControllerBaseTests
{
    [TestMethod]
    public async Task RedirectDefaultRouteToSwagger()
    {
        var httpClient = WebAppFactory.CreateDefaultClient();

        var response = await httpClient.GetAsync("");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}

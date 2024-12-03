using TestCadastroVendas.Dto.TestCadastroVendas;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http.Json;

namespace TestCadastroVendas.Test.Integration.Presentation.Api.Conttrollers;

[TestClass]
public class TestCadastroVendasControllerTests : ControllerBaseTests
{
    [TestMethod]
    public async Task CreateTestCadastroVendas()
    {
        var httpClient = WebAppFactory.CreateDefaultClient();

        var testCadastroVendasDTO = new TestCadastroVendasCreateDto("Meu teste integrado", CrossCutting.Enums.TestCadastroVendasType.Azure);
        var response = await httpClient.PostAsJsonAsync("testCadastroVendas", testCadastroVendasDTO);
        response.IsSuccessStatusCode.Should().Be(true);


        //var bodyResponse = await response.Content.ReadAsStringAsync();

        //var boileplateIdDto = await response.Content.ReadAsAsync<IdDTO>();

        //dynamic boileplateIdDto = JsonConvert.DeserializeObject<dynamic>(bodyResponse);

        //var response2 = await httpClient.GetAsync($"testCadastroVendas/{boileplateIdDto.Id}");

        //response2.IsSuccessStatusCode.Should().Be(true);

        //var bodyResponse2 = await response2.Content.ReadAsStringAsync();

        //var boileplateCreated = JsonConvert.DeserializeObject<TestCadastroVendasDTO>(bodyResponse2);

        //boileplateCreated.Should().Be(testCadastroVendasDTO);



    }
}

using TestCadastroVendas.CrossCutting.Enums;
using TestCadastroVendas.Dto.TestCadastroVendas;

namespace TestCadastroVendas.Test.Shared.Dto
{
    public static class CreateTestCadastroVendasDefaultTestDto
    {
        public static TestCadastroVendasCreateDto GetDefault() =>
            new("Nalfu", TestCadastroVendasType.AWS);
    }
}


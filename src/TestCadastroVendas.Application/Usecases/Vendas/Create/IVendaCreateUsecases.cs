using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Dto.Vendas;

namespace TestCadastroVendas.Application.Usecases.Vendas.Create
{
    public interface IVendaCreateUsecases
    {
        Task<ServiceResponse<Venda>> Execute(VendaCreateDto dto);
    }
}

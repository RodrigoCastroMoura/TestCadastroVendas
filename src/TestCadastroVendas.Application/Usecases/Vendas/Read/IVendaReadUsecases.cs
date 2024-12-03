using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Dto.Vendas;

namespace TestCadastroVendas.Application.Usecases.Vendas.Read
{
    public interface IVendaReadUsecases
    {
        Task<ServiceResponse<VendaDto>> Execute(int id);

        Task<ServiceResponse<PagedResponse<VendaDto>>> Execute();
    }
}

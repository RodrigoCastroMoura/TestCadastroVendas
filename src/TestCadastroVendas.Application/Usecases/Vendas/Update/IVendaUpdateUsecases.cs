using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Dto.Vendas;

namespace TestCadastroVendas.Application.Usecases.Vendas.Update
{
    public  interface IVendaUpdateUsecases
    {
        Task<ServiceResponse<Venda>> Execute(VendaUpdateDto dto);
    }
}

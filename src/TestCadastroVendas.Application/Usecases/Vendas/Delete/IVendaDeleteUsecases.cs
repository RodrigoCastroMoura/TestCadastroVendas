using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;

namespace TestCadastroVendas.Application.Usecases.Vendas.Delete
{
    public  interface IVendaDeleteUsecases
    {
        Task<ServiceResponse<Venda>>Execute(int id);
    }
}

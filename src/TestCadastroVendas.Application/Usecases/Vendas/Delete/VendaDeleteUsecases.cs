using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;

namespace TestCadastroVendas.Application.Usecases.Vendas.Delete
{
    public  class VendaDeleteUsecases : IVendaDeleteUsecases
    {
        private readonly IVendaRepository iVendaRepository;
        public VendaDeleteUsecases(IVendaRepository iVendaRepository)
        {
            this.iVendaRepository = iVendaRepository;
        }

        public async Task<ServiceResponse<Venda>>Execute(int id)
        {
            var response = new ServiceResponse<Venda>();

            try
            {
                await iVendaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

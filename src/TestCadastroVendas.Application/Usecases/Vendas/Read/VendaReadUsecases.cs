using AutoMapper;
using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Dto.Vendas;

namespace TestCadastroVendas.Application.Usecases.Vendas.Read
{
    public class VendaReadUsecases : IVendaReadUsecases
    {
        private readonly IVendaRepository iVendaRepository;
        private readonly IMapper mapper;

        public VendaReadUsecases(IVendaRepository iVendaRepository, IMapper mapper)
        {
            this.iVendaRepository = iVendaRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<VendaDto>> Execute(int id)
        {
            var response = new ServiceResponse<VendaDto>();

            try
            {
                var venda = await iVendaRepository.Get(id);

                response.Data = mapper.Map<VendaDto>(venda);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
           
        }
        public async Task<ServiceResponse<PagedResponse<VendaDto>>> Execute()
        {
            var response = new ServiceResponse<PagedResponse<VendaDto>>();

            try
            {
                var vendas = await iVendaRepository.GetAll();
                var vendasDto = mapper.Map<IEnumerable<VendaDto>>(vendas);
                var pagedResponse = new PagedResponse<VendaDto>(vendasDto, vendasDto.Count());
                response.Data = pagedResponse;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
        }

    }
}

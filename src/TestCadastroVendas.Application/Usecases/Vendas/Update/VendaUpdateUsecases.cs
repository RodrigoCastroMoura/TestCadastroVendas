using AutoMapper;
using FluentValidation.Results;
using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Domain.Validations.Vendas;
using TestCadastroVendas.Dto.Vendas;


namespace TestCadastroVendas.Application.Usecases.Vendas.Update
{
    public  class VendaUpdateUsecases : IVendaUpdateUsecases
    {
        private readonly IVendaRepository iVendaRepository;
        private VendaUpdateValidation vendaValidation;
        private readonly IMapper mapper;

        public VendaUpdateUsecases(IVendaRepository iVendaRepository, IMapper mapper)
        {
            this.iVendaRepository = iVendaRepository;
            this.mapper = mapper;
            vendaValidation = new VendaUpdateValidation();
}

        public async Task<ServiceResponse<Venda>> Execute(VendaUpdateDto dto)
        {
            var venda = mapper.Map<Venda>(dto);

            vendaValidation.Validate(venda);

            ValidationResult resultadoValidacao = vendaValidation.Validate(venda);

            var response = new ServiceResponse<Venda>();

            if (resultadoValidacao.IsValid)
            {
                try
                {
                    venda.Data = DateTime.Now;
                    await iVendaRepository.UpdateAsync(venda);
                    response.Data = venda;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                }

                return response;
            }
            else
            {
                response.Success = false;
                response.Message = resultadoValidacao.Errors[0].ErrorMessage;

                return response;
            }
        }
    }
}

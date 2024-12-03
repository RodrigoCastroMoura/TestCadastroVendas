using AutoMapper;
using FluentValidation.Results;
using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Domain.Validations.Vendas;
using TestCadastroVendas.Dto.Vendas;

namespace TestCadastroVendas.Application.Usecases.Vendas.Create
{
    public class VendaCreateUsecases : IVendaCreateUsecases
    {
        private readonly IVendaRepository iVendaRepository;
        private readonly IMapper mapper;
        private VendaCreateValidation vendaValidation;

        public VendaCreateUsecases(IVendaRepository iVendaRepository, IMapper mapper)
        {
            this.iVendaRepository = iVendaRepository;
            this.mapper = mapper;
            vendaValidation = new VendaCreateValidation();
        }

        public async Task<ServiceResponse<Venda>> Execute(VendaCreateDto dto)
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
                    await iVendaRepository.Add(venda);
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

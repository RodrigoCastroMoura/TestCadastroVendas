using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Dto;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Domain.Validations.Vendas;
using System.Linq.Expressions;

namespace TestCadastroVendas.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("Venda")]
[ApiController]
[Produces("application/json")]
public class VendaController :  ControllerBase
{
    private readonly IRepository<Venda> vendaRepository;
    private readonly IRepository<ItemVenda> itemVendaRepository;
    private readonly ILogger<VendaController> logger;


    private readonly IMapper mapper;
    private VendaCreateValidation vendaValidation;

    public VendaController(IRepository<Venda> vendaRepository,
                    IRepository<ItemVenda> itemVendaRepository,    
                    IMapper mapper,
                    ILogger<VendaController> logger)
    {
        this.vendaRepository = vendaRepository;
        this.itemVendaRepository = itemVendaRepository;
        this.mapper = mapper;
        this.logger = logger;
        vendaValidation = new VendaCreateValidation();
    }

    /// <summary>
    /// Criar Cadastro.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST/Venda
    ///     {
    ///       "dataVenda": "2024-12-01T10:30:00",  // Data e hora da venda (exemplo: "2024-12-01T10:30:00")
    ///       "cliente": "João da Silva",  // Nome do cliente (exemplo: "João da Silva")
    ///       "valorTotalVenda": 500.00,   // Valor total da venda (exemplo: 500.00)
    ///       "filial": "Filial A",        // Filial em que a venda foi realizada (exemplo: "Filial A")
    ///       "cancelado": false,          // Indica se a venda foi cancelada (exemplo: false)
    ///       "produtos": [                // Lista de produtos comprados
    ///          {
    ///              "produto": "Produto A",    // Nome do produto (exemplo: "Produto A")
    ///              "quantidade": 5,           // Quantidade do produto (exemplo: 5)
    ///              "valorUnitario": 100.00,   // Valor unitário do produto (exemplo: 100.00)
    ///              "desconto": 50.00,         // Desconto aplicado no produto (exemplo: 50.00)
    ///              "valorTotalItem": 450.00   // Valor total do item com desconto (exemplo: 450.00)
    ///          },
    ///          {
    ///              "produto": "Produto B",    // Nome do produto (exemplo: "Produto B")
    ///              "quantidade": 3,           // Quantidade do produto (exemplo: 3)
    ///              "valorUnitario": 20.00,    // Valor unitário do produto (exemplo: 20.00)
    ///              "desconto": 0.00,          // Desconto aplicado no produto (exemplo: 0.00)
    ///              "valorTotalItem": 60.00    // Valor total do item (exemplo: 60.00)
    ///          }
    ///      ]
    ///     }
    ///
    /// </remarks>
    /// <param name="vendaCreateDto"></param>
    /// <returns>A newly created TestCadastroVendas</returns>
    /// <response code="201">Returns the newly created testCadastroVendas</response>
    [ProducesResponseType(typeof(VendaDto), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<VendaDto>>> Create([FromBody] VendaCreateDto vendaCreateDto)
    {
        var venda = mapper.Map<Venda>(vendaCreateDto);
       
        vendaValidation.Validate(venda);

        ValidationResult resultadoValidacao = vendaValidation.Validate(venda);

        var response = new ServiceResponse<VendaDto>();

        if (resultadoValidacao.IsValid)
        {
            try
            {
                venda.AplicarDescontos();
                var produtos = venda.Produtos;
                venda.Produtos = null;
                await vendaRepository.AddAsync(venda);

                foreach (var item in produtos)
                {
                    item.NumeroVenda = venda.NumeroVenda;
                    await itemVendaRepository.AddAsync(item);
                }
                
                response.Data = mapper.Map<VendaDto>(venda);
                response.Data.Produtos = mapper.Map<List<ItemVendaDto>>(produtos);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            logger.LogInformation($"Venda realizada com sucesso.");

            return response;
        }
        else
        {
            response.Success = false;
            response.Message = resultadoValidacao.Errors[0].ErrorMessage;

            return BadRequest(response);
        }
    }

    /// <summary>
    /// Listar Venda por ID
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get/Venda/102030
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <returns>returns a testCadastroVendas</returns>
    /// <response code="200">Returns a testCadastroVendas </response>
    /// <response code="422">If testCadastroVendas not found </response>  
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VendaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ServiceResponse<VendaDto>>> GetById([FromRoute] int id)
    {
        var response = new ServiceResponse<VendaDto>();

        try
        {
            Expression<Func<ItemVenda, bool>> filtro = item => item.NumeroVenda == id;

            var venda = await vendaRepository.GetByIdAsync(id);

            if(venda == null)
                return NotFound();

            var produtos = await itemVendaRepository.GetAllAsync(filtro);

            response.Data = mapper.Map<VendaDto>(venda);
            response.Data.Produtos = mapper.Map<List<ItemVendaDto>>(produtos);
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            logger.LogError(ex.Message);
            return response;
        }
    }

    /// <summary>
    /// Listar todos os Vendas
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get/Venda
    ///
    /// </remarks>
    /// Listar todos os Vendas
    /// <returns>returns testCadastroVendass</returns>
    /// <response code="200">Returns Vendas </response> 
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VendaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ServiceResponse<PagedResponse<VendaDto>>>> GetAll()
    {
        var response = new ServiceResponse<PagedResponse<VendaDto>>();
        var vendasDto = new List<VendaDto>();

        try
        {
            var vendas = await vendaRepository.GetAllAsync();
            if (vendas == null)
                return NotFound();

            foreach (var item in vendas)
            {
                Expression<Func<ItemVenda, bool>> filtro = x => x.NumeroVenda == item.NumeroVenda;
                var produtos = await itemVendaRepository.GetAllAsync(filtro);
                var vendaDto = mapper.Map<VendaDto>(item);
                vendaDto.Produtos = mapper.Map<List<ItemVendaDto>>(produtos);

                vendasDto.Add(vendaDto);

            }

            var pagedResponse = new PagedResponse<VendaDto>(vendasDto, vendasDto.Count());
            response.Data = pagedResponse;

            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            logger.LogError(ex.Message);
            return response;
        }
    }

    /// <summary>
    /// Alterar Cadastro.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     Put/Venda
    ///     {
    ///       "numeroVenda": 12345,        // Número único da venda (exemplo: 12345)
    ///       "dataVenda": "2024-12-01T10:30:00",  // Data e hora da venda (exemplo: "2024-12-01T10:30:00")
    ///       "cliente": "João da Silva",  // Nome do cliente (exemplo: "João da Silva")
    ///       "valorTotalVenda": 500.00,   // Valor total da venda (exemplo: 500.00)
    ///       "filial": "Filial A",        // Filial em que a venda foi realizada (exemplo: "Filial A")
    ///       "cancelado": false,          // Indica se a venda foi cancelada (exemplo: false)
    ///       "produtos": [                // Lista de produtos comprados
    ///          {
    ///              "produto": "Produto A",    // Nome do produto (exemplo: "Produto A")
    ///              "quantidade": 5,           // Quantidade do produto (exemplo: 5)
    ///              "valorUnitario": 100.00,   // Valor unitário do produto (exemplo: 100.00)
    ///              "desconto": 50.00,         // Desconto aplicado no produto (exemplo: 50.00)
    ///              "valorTotalItem": 450.00   // Valor total do item com desconto (exemplo: 450.00)
    ///          },
    ///          {
    ///              "produto": "Produto B",    // Nome do produto (exemplo: "Produto B")
    ///              "quantidade": 3,           // Quantidade do produto (exemplo: 3)
    ///              "valorUnitario": 20.00,    // Valor unitário do produto (exemplo: 20.00)
    ///              "desconto": 0.00,          // Desconto aplicado no produto (exemplo: 0.00)
    ///              "valorTotalItem": 60.00    // Valor total do item (exemplo: 60.00)
    ///          }
    ///      ]
    ///     }
    ///     }
    ///
    /// </remarks>
    /// <param name="vendaUpdateDto"></param>
    /// <returns>A newly created TestCadastroVendas</returns>
    /// <response code="201">Returns the newly created testCadastroVendas</response>
    [ProducesResponseType(typeof(VendaDto), StatusCodes.Status200OK)]
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<VendaDto>>> Update([FromBody] VendaUpdateDto vendaUpdateDto)
    {
        var venda = mapper.Map<Venda>(vendaUpdateDto);

        vendaValidation.Validate(venda);

        ValidationResult resultadoValidacao = vendaValidation.Validate(venda);

        var response = new ServiceResponse<VendaDto>();

        if (resultadoValidacao.IsValid)
        {
            try
            {
                venda.AplicarDescontos();
                var produtos = venda.Produtos;
                venda.Produtos = null;
                await vendaRepository.UpdateAsync(venda);

                Expression<Func<ItemVenda, bool>> filtro = item => item.NumeroVenda == venda.NumeroVenda;
                await itemVendaRepository.DeleteAsync(filtro);

                foreach (var item in produtos)
                {
                    item.NumeroVenda = venda.NumeroVenda;
                    await itemVendaRepository.AddAsync(item);
                }

                response.Data = mapper.Map<VendaDto>(venda);
                response.Data.Produtos = mapper.Map<List<ItemVendaDto>>(produtos);


                logger.LogInformation($"Venda atualizada com sucesso.");
                return response;


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                logger.LogError(ex.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data.NumeroVenda }, response.Data);
        }
        else
        {
            response.Success = false;
            response.Message = resultadoValidacao.Errors[0].ErrorMessage;

            return BadRequest(response);
        }

    }

    /// <summary>
    /// Deletar Cadastro.
    /// </summary>
    /// <remarks>
    ///     Delete/Venda/102030
    /// </remarks>
    /// <returns>A newly created TestCadastroVendas</returns>
    /// <response code="201">Returns the newly created testCadastroVendas</response>
    [ProducesResponseType(typeof(ServiceResponse<Venda>), StatusCodes.Status200OK)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<Venda>>> Delete([FromRoute] int id)
    {
        var response = new ServiceResponse<Venda>();

        try
        {
            await vendaRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            logger.LogError(ex.Message);
            return BadRequest(response);
        }

        return response;
    }


}

using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCadastroVendas.Api.Controllers.v1;
using TestCadastroVendas.Domain.Data;
using TestCadastroVendas.Domain.Dto;
using TestCadastroVendas.Domain.Entities;
using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Infra.Mappers.TestCadastroVendasProfile;
using Xunit;

namespace TestCadastroVendas.Api.Tests.Controllers;

[TestClass]
public class VendaControllerTests
{
    private readonly VendaController _controller;
    private readonly IRepository<Venda> vendaRepository;
    private readonly IRepository<ItemVenda> itemVendaRepository;
    private readonly IMapper mapper;
    private readonly Faker _faker;
    private List<Venda> vendas;
    private Venda venda;

    public VendaControllerTests()
    {
        vendaRepository = Substitute.For<IRepository<Venda>>();
        itemVendaRepository = Substitute.For<IRepository<ItemVenda>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new VendasProfile()));
        mapper = config.CreateMapper();
        _controller = new VendaController(vendaRepository,itemVendaRepository,mapper);
        _faker = new Faker();  // Usando Bogus para gerar dados falsos
        venda = new Venda
        {
            NumeroVenda = 1,
            Cliente = _faker.Name.FullName(),
            ValorTotalVenda = 100m,
            DataVenda = System.DateTime.Now,
            Cancelado = false,
            Filial = "Filial A",
            Produtos = new List<ItemVenda>() { new ItemVenda {
                Produto = "Produto A",
                Quantidade = 5,
                ValorUnitario = 200m,
                Desconto = 10m,
                ValorTotalItem = 990m}
            }

        };
        vendas = new List<Venda>
            {
            new Venda { NumeroVenda = 1, Cliente = _faker.Name.FullName(), ValorTotalVenda = 100m, Produtos= new List<ItemVenda>()},
            new Venda { NumeroVenda = 2, Cliente = _faker.Name.FullName(), ValorTotalVenda = 200m, Produtos= new List<ItemVenda>() }
            };
    }

    [TestMethod]
    public async Task SHOULD_CONTROLLERS_GETALL_VENDAS()
    {
        // Arrange
           
        vendaRepository.GetAllAsync().Returns(vendas);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<PagedResponse<VendaDto>>>>();

        var okResult = result as ActionResult<ServiceResponse<PagedResponse<VendaDto>>>;
        okResult.Value.Data.Should().NotBeNull();
        okResult.Value.Data.TotalCount.Should().Be(2);
    }

    [TestMethod]
    public async Task SHOULD_CONTROLLERS_GETBYID_VENDAS()
    {
        // Arrange
           
        var vendaDto = mapper.Map<VendaDto>(venda);
        vendaRepository.GetByIdAsync(1).Returns(venda);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<VendaDto>>>();
        var okResult = result as ActionResult<ServiceResponse<VendaDto>>;

        okResult.Value.Data.NumeroVenda.Should().Be(1);
    }

    [TestMethod]
    public async Task SHOULD_CONTROLLERS_GETBYID_NOTFOUND_VENDAS()
    {
        // Arrange
        var vendaDto = mapper.Map<VendaDto>(venda);
        vendaRepository.GetByIdAsync(2).Returns(venda);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<VendaDto>>>();
        var okResult = result as ActionResult<ServiceResponse<VendaDto>>;

        ((Microsoft.AspNetCore.Mvc.StatusCodeResult)okResult.Result).StatusCode.Should().Be(404);
    }

    [TestMethod]
    public async Task SHOULD_CONTROLLERS_ADD_VENDAS()
    {
        // Arrange
        var vendaDto = mapper.Map<VendaCreateDto>(venda);
        vendaRepository.AddAsync(venda).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(vendaDto);

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<VendaDto>>>();
        var okResult = result as ActionResult<ServiceResponse<VendaDto>>;
        okResult.Value.Data.NumeroVenda.Should().Be(venda.NumeroVenda);
    }

    [TestMethod]
    public async Task SHOULD_CONTROLLERS_DELETE_VENDA()
    {
        // Arrange
        vendaRepository.DeleteAsync(1).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<Venda>>>();
        var okResult = result as ActionResult<ServiceResponse<Venda>>;

        okResult.Value.Success.Should().Be(true);
    }


    [TestMethod]
    public async Task SHOULD_CONTROLLERS_UPDATE_VENDAS()
    {
        // Arrange
        var vendaDto = mapper.Map<VendaUpdateDto>(venda);
        vendaRepository.UpdateAsync(venda).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(vendaDto);

        // Assert
        result.Should().BeOfType<ActionResult<ServiceResponse<VendaDto>>>();
        var okResult = result as ActionResult<ServiceResponse<VendaDto>>;
        okResult.Value.Data.NumeroVenda.Should().Be(venda.NumeroVenda);
    }

}


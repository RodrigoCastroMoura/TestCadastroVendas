using AutoMapper;
using TestCadastroVendas.Domain.Dto;

namespace TestCadastroVendas.Infra.Mappers.TestCadastroVendasProfile;

public class VendasProfile : Profile
{
    public VendasProfile()
    {
        CreateMap<Domain.Entities.Venda, VendaCreateDto>().ReverseMap();
        CreateMap<Domain.Entities.Venda, VendaDto>().ReverseMap();
        CreateMap<Domain.Entities.Venda, VendaUpdateDto>().ReverseMap();
        CreateMap<VendaCreateDto, Domain.Entities.Venda>().ReverseMap();
        CreateMap<VendaDto, Domain.Entities.Venda>().ReverseMap();
        CreateMap<VendaUpdateDto, Domain.Entities.Venda>().ReverseMap();
        CreateMap<ItemVendaDto, Domain.Entities.ItemVenda>().ReverseMap();


    }
}


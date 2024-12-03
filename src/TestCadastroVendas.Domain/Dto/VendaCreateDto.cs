using TestCadastroVendas.Domain.Entities;

namespace TestCadastroVendas.Domain.Dto;
public class VendaCreateDto
{
    public int NumeroVenda { get; set; }
    public DateTime DataVenda { get; set; }
    public string Cliente { get; set; }
    public decimal ValorTotalVenda { get; set; }
    public string Filial { get; set; }
    public List<ItemVenda> Produtos { get; set; }
    public bool Cancelado { get; set; }
}


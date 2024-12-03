namespace TestCadastroVendas.Domain.Entities;

public class Venda
{
    public Venda()
    {
    }

    public int NumeroVenda { get; set; }
    public DateTime DataVenda { get; set; }
    public string Cliente { get; set; }
    public decimal ValorTotalVenda { get; set; }
    public string Filial { get; set; }
    public List<ItemVenda> Produtos { get; set; }
    public bool Cancelado { get; set; }
    public ICollection<ItemVenda> ItensVenda { get; set; }
    public void AplicarDescontos()
    {
        foreach (var item in Produtos)
        {
            if (item.Quantidade > 20)
            {
                item.Quantidade = 20; 
            }

            if (item.Quantidade >= 10)
            {
                item.Desconto = item.Quantidade * item.ValorUnitario * 0.20m;
            }
            else if (item.Quantidade >= 4)
            {
                item.Desconto = item.Quantidade * item.ValorUnitario * 0.10m; 
            }
            else
            {
                item.Desconto = 0; // Sem desconto
            }

            item.ValorTotalItem = (item.Quantidade * item.ValorUnitario) - item.Desconto;
        }

        CalcularValorTotalVenda();
    }
    public void CalcularValorTotalVenda()
    {
        ValorTotalVenda = 0;
        foreach (var item in Produtos)
        {
            ValorTotalVenda += item.ValorTotalItem;
        }
    }

}

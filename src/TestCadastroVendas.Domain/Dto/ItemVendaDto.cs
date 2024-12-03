namespace TestCadastroVendas.Domain.Dto;
public class ItemVendaDto
{
    public int Id { get; set; }
    public int NumeroVenda { get; set; }
    public string Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorTotalItem { get; set; }
}


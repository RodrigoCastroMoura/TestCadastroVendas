using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace TestCadastroVendas.Dto.Vendas;

public class VendaDto
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Senha { get; set; }

    public DateTime Data { get; set; }
}



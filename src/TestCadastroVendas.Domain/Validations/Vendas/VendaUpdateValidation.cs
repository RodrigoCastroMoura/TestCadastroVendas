using FluentValidation;

namespace TestCadastroVendas.Domain.Validations.Vendas;

public class VendaUpdateValidation : AbstractValidator<Entities.Venda>
{
    public VendaUpdateValidation()
    {
        // Validação do número da venda
        RuleFor(venda => venda.NumeroVenda)
            .GreaterThan(0).WithMessage("O número da venda deve ser maior que zero.");

        // Validação da data da venda
        RuleFor(venda => venda.DataVenda)
            .NotEmpty()
            .WithMessage("A data da venda é obrigatória.");


        // Validação do cliente
        RuleFor(venda => venda.Cliente)
            .NotEmpty().WithMessage("O cliente é obrigatório.")
            .Length(2, 100).WithMessage("O nome do cliente deve ter entre 2 e 100 caracteres.");

        // Validação do valor total da venda
        RuleFor(venda => venda.ValorTotalVenda)
            .GreaterThan(0).WithMessage("O valor total da venda deve ser maior que zero.");

        // Validação da filial
        RuleFor(venda => venda.Filial)
            .NotEmpty().WithMessage("A filial é obrigatória.");

        // Validação de cancelamento
        RuleFor(venda => venda.Cancelado)
            .Must(cancelado => !cancelado).WithMessage("A venda não pode ser cancelada no momento da criação.");

        // Validação dos produtos (com regras baseadas na quantidade)
        RuleForEach(venda => venda.Produtos)
            .ChildRules(produto =>
            {
                produto.RuleFor(p => p.Produto)
                    .NotEmpty().WithMessage("O nome do produto é obrigatório.");

                produto.RuleFor(p => p.Quantidade)
                    .GreaterThan(0).WithMessage("A quantidade do produto deve ser maior que zero.")
                    .LessThanOrEqualTo(20).WithMessage("Não é possível vender mais que 20 unidades de um produto.");

                produto.RuleFor(p => p.ValorUnitario)
                    .GreaterThan(0).WithMessage("O valor unitário do produto deve ser maior que zero.");

                // Validação do desconto (deve ser zero ou maior, não pode ser negativo)
                produto.RuleFor(p => p.Desconto)
                    .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.");
            });

    }
}

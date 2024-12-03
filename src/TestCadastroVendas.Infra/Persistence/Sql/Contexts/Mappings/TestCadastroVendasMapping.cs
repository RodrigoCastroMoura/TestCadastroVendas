using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestCadastroVendas.Infra.Persistence.Sql.Contexts.Mappings;

public class TestCadastroVendasMapping : IEntityTypeConfiguration<Domain.Entities.Venda>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Venda> builder)
    {
        builder.ToTable("Vendas");
        builder.HasKey(c => c.NumeroVenda);
            

    }
}

public class ItemVendaMapping : IEntityTypeConfiguration<Domain.Entities.ItemVenda>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ItemVenda> builder)
    {
        builder.ToTable("ItemVendas");


        builder.HasKey(i => i.Id);  

        builder.Property(i => i.Produto)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ValorUnitario)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.ValorTotalItem)
            .HasColumnType("decimal(18,2)");

        // Outros mapeamentos...
    }
}



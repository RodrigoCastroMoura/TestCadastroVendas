using Microsoft.EntityFrameworkCore;
using TestCadastroVendas.Domain.Entities;

namespace TestCadastroVendas.Infra.Persistence.Sql.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItemVendas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venda>()
            .HasKey(c => c.NumeroVenda);

        modelBuilder.Entity<ItemVenda>()
           .HasKey(i => i.Id);  

        modelBuilder.Entity<ItemVenda>()
            .HasOne(i => i.Venda)  
            .WithMany(v => v.ItensVenda) 
            .HasForeignKey(i => i.NumeroVenda); 

    }

}


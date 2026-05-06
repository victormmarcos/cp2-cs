using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }

    public DbSet<Agencia> Agencias => Set<Agencia>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();
    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<MaquinaDeCartao> MaquinasDeCartao => Set<MaquinaDeCartao>();
    public DbSet<ReceberSalario> ReceberSalarios => Set<ReceberSalario>();
    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>()
            .HasDiscriminator<string>("TipoCliente")
            .HasValue<PessoaFisica>("PF")
            .HasValue<PessoaJuridica>("PJ");

        modelBuilder.Entity<Produto>()
            .Property(p => p.Ativo)
            .HasConversion<int>()
            .HasColumnType("NUMBER(1)");

        modelBuilder.Entity<ReceberSalario>()
            .Property(r => r.ExigeConvenioEmpregador)
            .HasConversion<int>()
            .HasColumnType("NUMBER(1)");

        modelBuilder.Entity<Produto>()
            .HasDiscriminator<string>("TipoProdutoBanco")
            .HasValue<Emprestimo>("EMPRESTIMO")
            .HasValue<MaquinaDeCartao>("MAQUINA_CARTAO")
            .HasValue<ReceberSalario>("RECEBER_SALARIO");

        modelBuilder.Entity<PessoaFisica>()
            .HasIndex(p => p.Cpf)
            .IsUnique();

        modelBuilder.Entity<PessoaJuridica>()
            .HasIndex(p => p.Cnpj)
            .IsUnique();

        modelBuilder.Entity<Agencia>()
            .HasIndex(a => a.Numero)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Agencia)
            .WithMany(a => a.Clientes)
            .HasForeignKey(c => c.AgenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contratacao>()
            .HasOne(c => c.Cliente)
            .WithMany(c => c.Contratacoes)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contratacao>()
            .HasOne(c => c.Produto)
            .WithMany(p => p.Contratacoes)
            .HasForeignKey(c => c.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emprestimo>().HasData(
    new Emprestimo
    {
        Id = 1,
        Nome = "Empréstimo Pessoal",
        ValorMinimo = 500,
        ValorMaximo = 50000,
        TaxaJurosMensal = 0.035m,
        PrazoMaximoMeses = 48,
        Ativo = true
    }
);

        modelBuilder.Entity<MaquinaDeCartao>().HasData(
            new MaquinaDeCartao
            {
                Id = 2,
                Nome = "Máquina de Cartão",
                MdrDebito = 0.0199m,
                MdrCreditoAVista = 0.0349m,
                MdrCreditoParcelado = 0.0499m,
                FaturamentoMinimoMensal = 3000,
                Ativo = true
            }
        );

        modelBuilder.Entity<ReceberSalario>().HasData(
            new ReceberSalario
            {
                Id = 3,
                Nome = "Receber Salário",
                ExigeConvenioEmpregador = true,
                Ativo = true
            }
        );
    }
}

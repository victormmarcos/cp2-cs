namespace ProjetoBanco.Api.Models;

public class PessoaFisica : Cliente
{
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public decimal RendaMensal { get; set; }
    public int Score { get; set; }
}

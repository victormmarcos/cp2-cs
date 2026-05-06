using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public class Emprestimo : Produto
{
    public decimal ValorMinimo { get; set; }
    public decimal ValorMaximo { get; set; }
    public decimal TaxaJurosMensal { get; set; }
    public int PrazoMaximoMeses { get; set; }

    public Emprestimo()
    {
        Tipo = TipoProduto.Emprestimo;
    }
}

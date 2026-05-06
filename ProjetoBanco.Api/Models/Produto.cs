using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public abstract class Produto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoProduto Tipo { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<Contratacao> Contratacoes { get; set; } = new List<Contratacao>();
}

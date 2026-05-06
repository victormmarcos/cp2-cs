using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public class Contratacao
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public long ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public StatusContratacao Status { get; set; } = StatusContratacao.Solicitada;
    public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataProcessamento { get; set; }
    public string? MotivoReprovacao { get; set; }

    public decimal? ValorSolicitado { get; set; }
    public int? PrazoMeses { get; set; }
}

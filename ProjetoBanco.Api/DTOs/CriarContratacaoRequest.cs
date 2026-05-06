namespace ProjetoBanco.Api.DTOs;

public record CriarContratacaoRequest(
    long ClienteId,
    long ProdutoId,
    decimal? ValorSolicitado,
    int? PrazoMeses
);

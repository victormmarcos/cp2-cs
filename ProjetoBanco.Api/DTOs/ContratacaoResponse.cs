using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.DTOs;

public record ContratacaoResponse(
    long Id,
    long ClienteId,
    long ProdutoId,
    StatusContratacao Status,
    DateTime DataSolicitacao,
    DateTime? DataProcessamento,
    string? MotivoReprovacao
);

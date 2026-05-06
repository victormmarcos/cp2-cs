namespace ProjetoBanco.Api.DTOs;

public record CriarPessoaJuridicaRequest(
    string Nome,
    string Email,
    long AgenciaId,
    string Cnpj,
    string RazaoSocial,
    decimal FaturamentoMensal,
    int Score
);

namespace ProjetoBanco.Api.DTOs;

public record CriarPessoaFisicaRequest(
    string Nome,
    string Email,
    long AgenciaId,
    string Cpf,
    DateTime DataNascimento,
    decimal RendaMensal,
    int Score
);

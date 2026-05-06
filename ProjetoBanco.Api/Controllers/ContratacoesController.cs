using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.DTOs;
using ProjetoBanco.Api.Messaging;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    private readonly BancoDbContext _db;
    private readonly RabbitMqPublisher _publisher;

    public ContratacoesController(BancoDbContext db, RabbitMqPublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> Solicitar(CriarContratacaoRequest request)
    {
        if (!await _db.Clientes.AnyAsync(c => c.Id == request.ClienteId))
            return NotFound(new { mensagem = "Cliente inexistente." });

        if (!await _db.Produtos.AnyAsync(p => p.Id == request.ProdutoId && p.Ativo))
            return BadRequest(new { mensagem = "Produto inexistente ou inativo." });

        var contratacao = new Contratacao
        {
            ClienteId = request.ClienteId,
            ProdutoId = request.ProdutoId,
            ValorSolicitado = request.ValorSolicitado,
            PrazoMeses = request.PrazoMeses
        };

        _db.Contratacoes.Add(contratacao);
        await _db.SaveChangesAsync();

        _publisher.PublicarContratacao(contratacao.Id);

        return AcceptedAtAction(nameof(BuscarPorId), new { id = contratacao.Id }, new { contratacao.Id, contratacao.Status });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> BuscarPorId(long id)
    {
        var contratacao = await _db.Contratacoes
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ContratacaoResponse(
                c.Id,
                c.ClienteId,
                c.ProdutoId,
                c.Status,
                c.DataSolicitacao,
                c.DataProcessamento,
                c.MotivoReprovacao
            ))
            .FirstOrDefaultAsync();

        return contratacao is null ? NotFound() : Ok(contratacao);
    }
}

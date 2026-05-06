using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.DTOs;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/agencias")]
public class AgenciasController : ControllerBase
{
    private readonly BancoDbContext _db;

    public AgenciasController(BancoDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarAgenciaRequest request)
    {
        if (await _db.Agencias.AnyAsync(a => a.Numero == request.Numero))
            return Conflict(new { mensagem = "Agência já cadastrada." });

        var agencia = new Agencia
        {
            Numero = request.Numero,
            Nome = request.Nome,
            Cidade = request.Cidade,
            Uf = request.Uf.ToUpperInvariant()
        };

        _db.Agencias.Add(agencia);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = agencia.Id }, agencia);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> BuscarPorId(long id)
    {
        var agencia = await _db.Agencias.FindAsync(id);
        return agencia is null ? NotFound() : Ok(agencia);
    }
}

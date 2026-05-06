using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.DTOs;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly BancoDbContext _db;

    public ClientesController(BancoDbContext db)
    {
        _db = db;
    }

    [HttpPost("pf")]
    public async Task<IActionResult> CriarPessoaFisica(CriarPessoaFisicaRequest request)
    {
        if (!await _db.Agencias.AnyAsync(a => a.Id == request.AgenciaId))
            return BadRequest(new { mensagem = "Agência inexistente." });

        if (await _db.PessoasFisicas.AnyAsync(p => p.Cpf == request.Cpf))
            return Conflict(new { mensagem = "CPF já cadastrado." });

        var cliente = new PessoaFisica
        {
            Nome = request.Nome,
            Email = request.Email,
            AgenciaId = request.AgenciaId,
            Cpf = request.Cpf,
            DataNascimento = request.DataNascimento,
            RendaMensal = request.RendaMensal,
            Score = request.Score
        };

        _db.PessoasFisicas.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }

    [HttpPost("pj")]
    public async Task<IActionResult> CriarPessoaJuridica(CriarPessoaJuridicaRequest request)
    {
        if (!await _db.Agencias.AnyAsync(a => a.Id == request.AgenciaId))
            return BadRequest(new { mensagem = "Agência inexistente." });

        if (await _db.PessoasJuridicas.AnyAsync(p => p.Cnpj == request.Cnpj))
            return Conflict(new { mensagem = "CNPJ já cadastrado." });

        var cliente = new PessoaJuridica
        {
            Nome = request.Nome,
            Email = request.Email,
            AgenciaId = request.AgenciaId,
            Cnpj = request.Cnpj,
            RazaoSocial = request.RazaoSocial,
            FaturamentoMensal = request.FaturamentoMensal,
            Score = request.Score
        };

        _db.PessoasJuridicas.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> BuscarPorId(long id)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Agencia)
            .FirstOrDefaultAsync(c => c.Id == id);

        return cliente is null ? NotFound() : Ok(cliente);
    }
}

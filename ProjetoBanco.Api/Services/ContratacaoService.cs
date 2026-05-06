using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Enums;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Services;

public class ContratacaoService
{
    private readonly BancoDbContext _db;

    public ContratacaoService(BancoDbContext db)
    {
        _db = db;
    }

    public async Task ProcessarAsync(long contratacaoId)
    {
        var contratacao = await _db.Contratacoes
            .Include(c => c.Cliente)
            .Include(c => c.Produto)
            .FirstOrDefaultAsync(c => c.Id == contratacaoId);

        if (contratacao is null) return;

        contratacao.Status = StatusContratacao.EmProcessamento;
        await _db.SaveChangesAsync();

        try
        {
            switch (contratacao.Produto)
            {
                case Emprestimo emprestimo:
                    ProcessarEmprestimo(contratacao, emprestimo);
                    break;

                case MaquinaDeCartao maquina:
                    ProcessarMaquinaDeCartao(contratacao, maquina);
                    break;

                default:
                    contratacao.Status = StatusContratacao.Reprovada;
                    contratacao.MotivoReprovacao = "Produto não implementado para processamento.";
                    break;
            }

            contratacao.DataProcessamento = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            contratacao.Status = StatusContratacao.ErroProcessamento;
            contratacao.MotivoReprovacao = ex.Message;
            contratacao.DataProcessamento = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    private static void ProcessarEmprestimo(Contratacao contratacao, Emprestimo produto)
    {
        var valor = contratacao.ValorSolicitado ?? 0;
        var prazo = contratacao.PrazoMeses ?? 0;

        if (valor < produto.ValorMinimo || valor > produto.ValorMaximo)
        {
            Reprovar(contratacao, "Valor solicitado fora dos limites do produto.");
            return;
        }

        if (prazo <= 0 || prazo > produto.PrazoMaximoMeses)
        {
            Reprovar(contratacao, "Prazo inválido para o empréstimo.");
            return;
        }

        var renda = contratacao.Cliente switch
        {
            PessoaFisica pf => pf.RendaMensal,
            PessoaJuridica pj => pj.FaturamentoMensal,
            _ => 0
        };

        var score = contratacao.Cliente switch
        {
            PessoaFisica pf => pf.Score,
            PessoaJuridica pj => pj.Score,
            _ => 0
        };

        var parcelaEstimativa = valor / prazo;

        if (score < 600)
        {
            Reprovar(contratacao, "Score insuficiente para empréstimo.");
            return;
        }

        if (parcelaEstimativa > renda * 0.30m)
        {
            Reprovar(contratacao, "Parcela estimada acima de 30% da renda/faturamento.");
            return;
        }

        Aprovar(contratacao);
    }

    private static void ProcessarMaquinaDeCartao(Contratacao contratacao, MaquinaDeCartao produto)
    {
        var faturamento = contratacao.Cliente switch
        {
            PessoaJuridica pj => pj.FaturamentoMensal,
            PessoaFisica pf => pf.RendaMensal,
            _ => 0
        };

        var score = contratacao.Cliente switch
        {
            PessoaJuridica pj => pj.Score,
            PessoaFisica pf => pf.Score,
            _ => 0
        };

        if (faturamento < produto.FaturamentoMinimoMensal)
        {
            Reprovar(contratacao, "Faturamento/renda mensal abaixo do mínimo para máquina de cartão.");
            return;
        }

        if (score < 500)
        {
            Reprovar(contratacao, "Score insuficiente para máquina de cartão.");
            return;
        }

        Aprovar(contratacao);
    }

    private static void Aprovar(Contratacao contratacao)
    {
        contratacao.Status = StatusContratacao.Aprovada;
        contratacao.MotivoReprovacao = null;
    }

    private static void Reprovar(Contratacao contratacao, string motivo)
    {
        contratacao.Status = StatusContratacao.Reprovada;
        contratacao.MotivoReprovacao = motivo;
    }
}

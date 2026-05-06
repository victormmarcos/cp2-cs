using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public class MaquinaDeCartao : Produto
{
    public decimal MdrDebito { get; set; }
    public decimal MdrCreditoAVista { get; set; }
    public decimal MdrCreditoParcelado { get; set; }
    public decimal FaturamentoMinimoMensal { get; set; }

    public MaquinaDeCartao()
    {
        Tipo = TipoProduto.MaquinaDeCartao;
    }
}

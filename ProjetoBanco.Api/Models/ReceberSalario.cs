using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public class ReceberSalario : Produto
{
    public bool ExigeConvenioEmpregador { get; set; } = true;

    public ReceberSalario()
    {
        Tipo = TipoProduto.ReceberSalario;
    }
}

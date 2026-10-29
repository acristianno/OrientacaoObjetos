using UtfBikes.Negocio.Validacao;

namespace UtfBikes.Negocio.Modelo;

public sealed class Servico {
  public OrdemServico Ordem { get; }
  public string Descricao { get; }
  public decimal Valor { get; }
  public bool Ativo { get; private set; } = true;

  // Composição: só a OrdemServico cria seus serviços.
  internal Servico(OrdemServico ordem, string descricao, decimal valor) {
    Ordem = ordem ?? throw new ArgumentNullException(nameof(ordem));
    Descricao = Validador.TextoObrigatorio(descricao, "Descrição");
    Valor = Validador.ValorNaoNegativo(valor, "Valor");
  }

  internal void Descartar() => Ativo = false;
}

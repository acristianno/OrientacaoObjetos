namespace UtfBikes.Negocio.Validacao;

public static class Validador {
  public static string TextoObrigatorio(string valor, string campo) {
    if (string.IsNullOrWhiteSpace(valor))
      throw new ArgumentException($"{campo} obrigatório.");
    return valor.Trim();
  }

  public static decimal ValorNaoNegativo(decimal valor, string campo) {
    if (valor < 0)
      throw new ArgumentException($"{campo} não pode ser negativo.");
    return valor;
  }
}

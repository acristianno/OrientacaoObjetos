using UtfBikes.Negocio.Validacao;

namespace UtfBikes.Negocio.Modelo;

public sealed class Bicicleta {
  public Guid BicicletaId { get; }
  public string Modelo { get; private set; }
  public Cliente? Cliente { get; private set; }

  public Bicicleta(string modelo, Guid? bicicletaId = null) {
    BicicletaId = bicicletaId ?? Guid.NewGuid();
    Modelo = Validador.TextoObrigatorio(modelo, "Modelo");
  }

  internal void DefinirCliente(Cliente cliente) => Cliente = cliente;

  internal void RemoverCliente() => Cliente = null;

  public override bool Equals(object? obj) =>
    obj is Bicicleta outra && BicicletaId == outra.BicicletaId;

  public override int GetHashCode() => BicicletaId.GetHashCode();
}

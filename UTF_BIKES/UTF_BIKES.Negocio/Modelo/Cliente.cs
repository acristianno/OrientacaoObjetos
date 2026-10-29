using UtfBikes.Negocio.Validacao;

namespace UtfBikes.Negocio.Modelo;

public sealed class Cliente {
  private readonly List<Bicicleta> _bicicletas = new();

  public Guid ClienteId { get; }
  public string Nome { get; private set; }
  public string Telefone { get; private set; }
  public IReadOnlyCollection<Bicicleta> Bicicletas => _bicicletas.AsReadOnly();

  public Cliente(string nome, string telefone, Guid? clienteId = null) {
    ClienteId = clienteId ?? Guid.NewGuid();
    Nome = Validador.TextoObrigatorio(nome, "Nome");
    Telefone = Validador.TextoObrigatorio(telefone, "Telefone");
  }

  public void Alterar(string? nome = null, string? telefone = null) {
    if (nome is null && telefone is null)
      throw new ArgumentException("Informe ao menos o nome ou o telefone.");
    var novoNome = nome is null
      ? Nome
      : Validador.TextoObrigatorio(nome, "Nome");
    var novoTelefone = telefone is null
      ? Telefone
      : Validador.TextoObrigatorio(telefone, "Telefone");
    Nome = novoNome;
    Telefone = novoTelefone;
  }

  // Agregação: a bicicleta é apenas vinculada ao cliente; ela é criada
  // fora dele e continua existindo depois de desvinculada.
  public void VincularBicicleta(Bicicleta bicicleta) {
    ArgumentNullException.ThrowIfNull(bicicleta);
    if (_bicicletas.Contains(bicicleta))
      throw new InvalidOperationException(
        "Bicicleta já vinculada a este cliente.");
    if (bicicleta.Cliente is not null)
      throw new InvalidOperationException(
        "Bicicleta pertence a outro cliente. Desvincule antes.");
    _bicicletas.Add(bicicleta);
    bicicleta.DefinirCliente(this);
  }

  public void DesvincularBicicleta(Bicicleta bicicleta) {
    if (bicicleta is null || !_bicicletas.Remove(bicicleta))
      throw new InvalidOperationException(
        "Bicicleta não está vinculada a este cliente.");
    bicicleta.RemoverCliente();
  }

  public override bool Equals(object? obj) =>
    obj is Cliente outro && ClienteId == outro.ClienteId;

  public override int GetHashCode() => ClienteId.GetHashCode();
}

using UtfBikes.Negocio.Modelo;

namespace UtfBikes.Negocio.Servicos;

public sealed class ServicoClientes {
  private readonly List<Cliente> _clientes = new();

  public IReadOnlyList<Cliente> Listar() => _clientes.AsReadOnly();

  public Cliente Obter(Guid id) =>
    _clientes.FirstOrDefault(c => c.ClienteId == id)
      ?? throw new KeyNotFoundException("Cliente não encontrado.");

  public Cliente Cadastrar(string nome, string telefone) {
    var cliente = new Cliente(nome, telefone);
    _clientes.Add(cliente);
    return cliente;
  }

  public void Alterar(Guid id, string? nome = null, string? telefone = null) =>
    Obter(id).Alterar(nome, telefone);

  public void Remover(Guid id) {
    var cliente = Obter(id);
    if (cliente.Bicicletas.Count > 0)
      throw new InvalidOperationException(
        "Cliente com bicicletas vinculadas não pode ser removido.");
    _clientes.Remove(cliente);
  }
}

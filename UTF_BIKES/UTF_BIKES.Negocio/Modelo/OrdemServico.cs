namespace UtfBikes.Negocio.Modelo;

public sealed class OrdemServico {
  private readonly List<Servico> _servicos = new();

  public Guid OrdemServicoId { get; }
  public Bicicleta Bicicleta { get; }
  public StatusOrdem Status { get; private set; } = StatusOrdem.Aberta;
  public IReadOnlyCollection<Servico> Servicos => _servicos.AsReadOnly();
  public decimal Total => _servicos.Sum(s => s.Valor);

  public OrdemServico(Bicicleta bicicleta, Guid? ordemServicoId = null) {
    OrdemServicoId = ordemServicoId ?? Guid.NewGuid();
    Bicicleta = bicicleta
      ?? throw new ArgumentNullException(nameof(bicicleta));
    if (bicicleta.Cliente is null)
      throw new InvalidOperationException(
        "Bicicleta deve pertencer a um cliente.");
  }

  // Composição: a ordem cria, remove e descarta os próprios serviços.
  public Servico AdicionarServico(string descricao, decimal valor) {
    ExigirAberta();
    var servico = new Servico(this, descricao, valor);
    _servicos.Add(servico);
    return servico;
  }

  public void RemoverServico(Servico servico) {
    ExigirAberta();
    if (servico is null || !_servicos.Remove(servico))
      throw new InvalidOperationException(
        "Serviço não encontrado nesta ordem.");
    servico.Descartar();
  }

  // Encerrar preserva os serviços como histórico do atendimento.
  public void Encerrar() {
    ExigirAberta();
    Status = StatusOrdem.Encerrada;
  }

  // Cancelar acaba com a ordem: os serviços deixam de existir.
  public void Cancelar() {
    ExigirAberta();
    foreach (var servico in _servicos)
      servico.Descartar();
    _servicos.Clear();
    Status = StatusOrdem.Cancelada;
  }

  private void ExigirAberta() {
    if (Status != StatusOrdem.Aberta)
      throw new InvalidOperationException(
        $"Ordem {Status.ToString().ToLower()} não pode ser alterada.");
  }

  public override bool Equals(object? obj) =>
    obj is OrdemServico outra && OrdemServicoId == outra.OrdemServicoId;

  public override int GetHashCode() => OrdemServicoId.GetHashCode();
}

using UtfBikes.Negocio.Modelo;
using UtfBikes.Negocio.Servicos;

namespace UtfBikes.Menu;

public sealed class MenuClientes {
  private readonly ServicoClientes _servico;

  public MenuClientes(ServicoClientes servico) {
    _servico = servico;
  }

  public void Executar() {
    while (true) {
      Console.WriteLine();
      Console.WriteLine("=== Clientes ===");
      Console.WriteLine("1 - Cadastrar");
      Console.WriteLine("2 - Listar");
      Console.WriteLine("3 - Consultar");
      Console.WriteLine("4 - Alterar");
      Console.WriteLine("5 - Remover");
      Console.WriteLine("0 - Sair");
      var opcao = Ler("Opção: ");
      if (opcao == "0") return;

      try {
        switch (opcao) {
          case "1": Cadastrar(); break;
          case "2": Listar(); break;
          case "3": Consultar(); break;
          case "4": Alterar(); break;
          case "5": Remover(); break;
          default: Console.WriteLine("Opção inválida."); break;
        }
      }
      catch (Exception ex) when (ex is ArgumentException
          or InvalidOperationException or KeyNotFoundException) {
        Console.WriteLine($"Operação não concluída: {ex.Message}");
      }
    }
  }

  private void Cadastrar() {
    var nome = Ler("Nome: ");
    var telefone = Ler("Telefone: ");
    var cliente = _servico.Cadastrar(nome, telefone);
    Console.WriteLine($"Cliente cadastrado: {cliente.Nome}");
  }

  private void Listar() {
    var clientes = _servico.Listar();
    if (clientes.Count == 0) {
      Console.WriteLine("Nenhum cliente cadastrado.");
      return;
    }
    for (var i = 0; i < clientes.Count; i++)
      Console.WriteLine($"{i + 1} - {clientes[i].Nome} | {clientes[i].Telefone}");
  }

  private void Consultar() {
    var cliente = Selecionar();
    if (cliente is null) return;
    Console.WriteLine($"Id: {cliente.ClienteId}");
    Console.WriteLine($"Nome: {cliente.Nome}");
    Console.WriteLine($"Telefone: {cliente.Telefone}");
    Console.WriteLine($"Bicicletas: {cliente.Bicicletas.Count}");
  }

  private void Alterar() {
    var cliente = Selecionar();
    if (cliente is null) return;
    var nome = Ler($"Novo nome (Enter mantém '{cliente.Nome}'): ");
    var telefone = Ler($"Novo telefone (Enter mantém '{cliente.Telefone}'): ");
    _servico.Alterar(
      cliente.ClienteId,
      nome == "" ? null : nome,
      telefone == "" ? null : telefone);
    Console.WriteLine("Cliente alterado.");
  }

  private void Remover() {
    var cliente = Selecionar();
    if (cliente is null) return;
    _servico.Remover(cliente.ClienteId);
    Console.WriteLine($"Cliente removido: {cliente.Nome}");
  }

  // Lista os clientes e devolve o escolhido pelo número digitado.
  private Cliente? Selecionar() {
    var clientes = _servico.Listar();
    if (clientes.Count == 0) {
      Console.WriteLine("Nenhum cliente cadastrado.");
      return null;
    }
    Listar();
    var texto = Ler("Número do cliente: ");
    if (!int.TryParse(texto, out var numero)
        || numero < 1 || numero > clientes.Count) {
      Console.WriteLine("Número inválido.");
      return null;
    }
    return clientes[numero - 1];
  }

  private static string Ler(string pergunta) {
    Console.Write(pergunta);
    return Console.ReadLine() ?? "";
  }
}

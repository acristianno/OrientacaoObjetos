using UtfBikes.Negocio.Modelo;

// ===== Agregação: Cliente e Bicicleta =====
var carlos = new Cliente("Carlos Souza", "(41) 99999-0000");
var bicicleta = new Bicicleta("Caloi Elite 29");
Console.WriteLine($"Bicicleta criada sem dono: {bicicleta.Cliente is null}");

carlos.VincularBicicleta(bicicleta);
Console.WriteLine($"Vinculada: {carlos.Bicicletas.Count} bicicleta(s) | dono: {bicicleta.Cliente!.Nome}");

Tentar(() => carlos.VincularBicicleta(bicicleta));

carlos.DesvincularBicicleta(bicicleta);
Console.WriteLine($"Desvinculada: {carlos.Bicicletas.Count} bicicleta(s) | "
  + $"modelo ainda existe: {bicicleta.Modelo} | dono: {bicicleta.Cliente?.Nome ?? "nenhum"}");

Tentar(() => new OrdemServico(bicicleta));

var ana = new Cliente("Ana Lima", "(41) 98888-1111");
ana.VincularBicicleta(bicicleta);
Console.WriteLine($"Nova dona: {bicicleta.Cliente!.Nome}");

// ===== Composição: OrdemServico e Servico =====
var ordem = new OrdemServico(bicicleta);
var freios = ordem.AdicionarServico("Regulagem de freios", 50m);
var camara = ordem.AdicionarServico("Troca de câmara", 35m);
Console.WriteLine($"Ordem {ordem.Status}: {ordem.Servicos.Count} serviço(s) | "
  + $"total {ordem.Total:C} | pertence à ordem: {freios.Ordem == ordem}");

ordem.RemoverServico(camara);
Console.WriteLine($"Serviço removido: ativo={camara.Ativo} | ordem tem {ordem.Servicos.Count}");

Tentar(() => ordem.AdicionarServico("Lubrificação", -10m));

ordem.Cancelar();
Console.WriteLine($"Ordem {ordem.Status}: {ordem.Servicos.Count} serviço(s) | "
  + $"freios ativo={freios.Ativo}");
Console.WriteLine($"Bicicleta continua existindo: {bicicleta.Modelo} | dona: {bicicleta.Cliente!.Nome}");

Tentar(() => ordem.AdicionarServico("Lubrificação", 20m));
Tentar(() => ordem.Encerrar());

static void Tentar(Action acao) {
  try {
    acao();
    Console.WriteLine("ERRO: a operação deveria ser recusada.");
  }
  catch (Exception ex) when (ex is ArgumentException or InvalidOperationException) {
    Console.WriteLine($"Recusado: {ex.Message}");
  }
}

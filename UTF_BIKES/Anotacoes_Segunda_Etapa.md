# UTF_BIKES — Anotações da segunda etapa (agregação e composição)

## Pergunta-chave
A parte existe sem o todo? Quem cria, quem mantém e quem remove? Uma lista dentro de outra classe, por si só, não define o tipo da relação: quem define é o domínio.

## Agregação: Cliente e Bicicleta
- **Por quê:** a bicicleta existe sem o cliente. Ela é criada fora dele (`new Bicicleta("Caloi Elite 29")`) e pode mudar de dono, por exemplo quando é vendida.
- **Como:** `Cliente` tem a lista privada `_bicicletas`, exposta como `IReadOnlyCollection`. Os métodos `VincularBicicleta` e `DesvincularBicicleta` são os únicos que a alteram. O cliente não cria a bicicleta.
- **Regras:**
  - não vincular a mesma bicicleta duas vezes;
  - não vincular uma bicicleta que pertence a outro cliente (é preciso desvincular antes);
  - não desvincular uma bicicleta que não está vinculada.
- **Demonstração:** a bicicleta é criada sem dono, vinculada ao Carlos, desvinculada e vinculada à Ana. O modelo e o objeto continuam existindo o tempo todo.
- **Regra de negócio do tema:** "bicicleta deve pertencer a cliente" é verificada ao abrir a ordem. `new OrdemServico(bicicletaSemDono)` é recusado.

## Composição: OrdemServico e Servico
- **Por quê:** um serviço só faz sentido dentro de uma ordem. "Troca de câmara" sozinho não é um atendimento. Ele registra o que foi feito naquela ordem, com aquele valor.
- **Como:**
  - o construtor de `Servico` é `internal`, então só a `OrdemServico` cria serviços, em `AdicionarServico(descricao, valor)`;
  - `Servico.Ordem` é somente leitura;
  - `RemoverServico` e `Cancelar` chamam `Servico.Descartar()`, que marca `Ativo = false`.
- **Efeito do fim do principal:**
  - `Cancelar()`: os serviços são descartados (`Ativo = false`) e a lista é esvaziada. A bicicleta e o cliente permanecem intactos.
  - `Encerrar()`: preserva os serviços como histórico e total do atendimento. É de propósito, porque a oficina precisa do valor cobrado. Só `Cancelar` acaba com os componentes.
- **Demonstração:** a ordem cria dois serviços, remove um (`ativo=False`) e depois é cancelada (0 serviços, `freios ativo=False`). A bicicleta continua existindo, com a Ana como dona.

## Operações inválidas demonstradas (e por que são recusadas)
| Operação | Por que é recusada |
|---|---|
| Vincular a mesma bicicleta duas vezes | O cliente já a possui, e duplicar quebraria a lista |
| Abrir ordem para bicicleta sem dono | A regra "bicicleta deve pertencer a cliente" |
| Serviço com valor negativo | Valor não pode ser negativo (validação do `Servico`) |
| Adicionar serviço ou encerrar ordem cancelada | Ordem que não está aberta não pode ser alterada |
| `new Servico(...)` fora da ordem | Erro de compilação (CS1729): o construtor é `internal` |

## Mudanças em relação à primeira etapa
- `Bicicleta` não exige mais `Cliente` no construtor. O dono é definido só por `Cliente.VincularBicicleta` (métodos `internal`).
- `OrdemServico.AdicionarServico` agora recebe descrição e valor e cria o `Servico` por dentro. Antes recebia um `Servico` pronto.
- Novo status `Cancelada` e os métodos `RemoverServico` e `Cancelar`.
- A regra "ordem encerrada não recebe serviço" continua valendo, agora para qualquer ordem que não esteja aberta.

## Descobertas adicionais (opcional)
- Bicicleta que pertence a outro cliente não pode ser vinculada sem desvincular antes. Isso evita relacionamento inconsistente (dois donos).
- `Encerrar` e `Cancelar` têm efeitos diferentes sobre os componentes. Isso mostra que o "fim do todo" pode significar coisas distintas no domínio.

## Como executar
```
dotnet run --project UTF_BIKES.Console
```
O `Program.cs` é só o roteiro de demonstração, sem regra de negócio.

# UTF_BIKES — Anotações da primeira etapa

## Tema do projeto
Sistema de Oficina de Bicicletas (UTF_BIKES).

## Descrição breve
Aplicação de console para uma oficina de bicicletas administrar os serviços de manutenção de seus clientes. O sistema resolve a desorganização do atendimento: quem é o cliente, qual bicicleta trouxe, quais serviços foram feitos e se o atendimento já foi encerrado. Destina-se ao atendente ou dono da oficina.

## Funcionalidades principais
- Cadastrar clientes
- Cadastrar bicicletas (sempre ligadas a um cliente)
- Abrir ordens de serviço
- Registrar serviços na ordem
- Encerrar atendimentos

## Conceito 1 — Encapsulamento com validação (Etapas 2 e 4)
- **O que é:** o objeto protege o próprio estado. O construtor valida os dados, os setters são `private` e só métodos da classe alteram o estado.
- **Como o guia usa:** `Organizador` não existe com nome ou e-mail inválido. O `Validador` estático concentra as regras e lança `ArgumentException`.
- **Aplicação na oficina:** `Servico` recusa valor negativo e descrição vazia. `Cliente` recusa nome vazio. `Bicicleta` não existe sem `Cliente`, o que garante a regra "bicicleta deve pertencer a cliente".

## Conceito 2 — Coleção protegida e regras no próprio objeto (Etapa 4)
- **O que é:** a lista interna é `private` e aparece para fora como `IReadOnlyCollection`. Só métodos da classe a modificam, e o objeto recusa sozinho operações inválidas (`InvalidOperationException`).
- **Como o guia usa:** `Evento` guarda `_participantes` e só `Inscrever` e `CancelarInscricao` a alteram. Lotação e duplicidade são recusadas pelo próprio evento.
- **Aplicação na oficina:** `OrdemServico` guarda `_servicos` e só `AdicionarServico` a altera. A regra "ordem encerrada não recebe serviço" fica dentro da ordem.

## Identidade (complemento)
Entidades têm `Guid` estável e `Equals`/`GetHashCode` baseados nele (Etapa 4). Já aplicado em `Cliente`, `Bicicleta` e `OrdemServico`.

## Experimento executado
Etapa 4, experimento 3: acrescentei `ordem.Servicos.Add(new Servico("Teste", 1m));` no `Program.cs`.
Resultado: o compilador recusou com **CS1061** (`IReadOnlyCollection<Servico>` não tem `Add`). Só `AdicionarServico` consegue incluir serviço, e ele aplica a regra do status. A linha foi removida em seguida.

## Primeira implementação (já feita)
Solução `UTF_BIKES` com três projetos (Console, Negocio, Persistencia), como na Etapa 1. Pastas do Negócio: Modelo, Contratos, Servicos e Validacao.

| Classe | Papel | Regras |
|---|---|---|
| `Validador` | validações reaproveitáveis | texto obrigatório, valor não negativo |
| `Cliente` | cadastro do cliente | nome e telefone obrigatórios |
| `Bicicleta` | pertence a um cliente | modelo obrigatório, cliente obrigatório |
| `Servico` | item de uma ordem | descrição obrigatória, valor ≥ 0 |
| `OrdemServico` | atendimento com status e serviços | `AdicionarServico`, `Encerrar`, ordem encerrada não recebe serviço, não encerra duas vezes |

O `Program.cs` serve como roteiro de teste e mostra os casos válidos e três recusas: valor negativo, bicicleta sem cliente e serviço em ordem encerrada.

## Sugestões ou descobertas adicionais (opcional)
- `Encerrar` também recusa uma ordem já encerrada (a Etapa 4 não tem regra equivalente).
- `Servico` ficou imutável (sem `set`): uma vez registrado, não muda.

## Próximos passos (não feitos agora)
Gerenciador e CRUD, menus, persistência (contrato da Etapa 5 e JSON), e a segunda etapa (agregação e composição).

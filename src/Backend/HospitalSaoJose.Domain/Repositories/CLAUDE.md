# Repositories

Só interfaces. A implementação fica em `Infrastructure/DataAccess/Repositories`.

## A tríade

Uma pasta por agregado, com até três interfaces:

| Interface | Para quê | Rastreamento do EF |
|---|---|---|
| `I<X>ReadOnlyRepository` | consultas que só alimentam a resposta | `AsNoTracking()`, com os `Include` necessários |
| `I<X>WriteOnlyRepository` | `Add` de entidade nova | — |
| `I<X>UpdateOnlyRepository` | `GetById` **rastreado** + `Update` | rastreado, sem `AsNoTracking()` |

Regra prática: se o use case vai **alterar** a entidade (inclusive soft delete), ele busca pelo
`UpdateOnly`. Se só vai devolvê-la, usa o `ReadOnly`. Um use case pode injetar os dois.

## Colisão de nomes

O namespace `HospitalSaoJose.Domain.Repositories.User` esconde o tipo
`HospitalSaoJose.Domain.Entities.User`. Dentro destas interfaces escreva `Entities.User`,
`Entities.Profile`, etc. — como já está em `User/IUserReadOnlyRepository.cs`.

## Convenções de assinatura

- Consulta que devolve zero ou um: `Task<Entities.X?>`.
- Consulta paginada: `Task<PagedResult<Entities.X>>` recebendo um `<X>FilterDto` de `../Dtos`.
- Checagem de existência: `Task<bool> ExistActive...`, sempre filtrando por `Active`.
- Para unicidade em edição existe a variante `...ForOtherX(valor, Guid id)`, que ignora o
  próprio registro.
- `Update` é `void`: quem persiste é `IUnitOfWork.Commit()`, chamado pelo use case.

## Não faça

- Não exponha `IQueryable` nem tipos do EF Core — este projeto não referencia EF.
- Não crie `Delete`: exclusão é `Active = false` via `UpdateOnly`.

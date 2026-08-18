# DataAccess

EF Core + Npgsql. Tudo aqui é **`internal`**: a única superfície pública da Infrastructure é
`DependencyInjectionExtension`, `DatabaseMigration` e `DatabaseSeeder`.

## Regras

- `internal sealed class <X>Repository` implementa as **três** interfaces do agregado
  (`ReadOnly`, `WriteOnly`, `UpdateOnly`) e é registrada três vezes no DI, apontando para a
  mesma classe.
- Quando `ReadOnly` e `UpdateOnly` têm um `GetById` de mesma assinatura, use **implementação
  explícita de interface** para diferenciar o comportamento:

```csharp
async Task<Category?> ICategoryReadOnlyRepository.GetById(Guid id) =>
    await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Active && c.Id == id);

async Task<Category?> ICategoryUpdateOnlyRepository.GetById(Guid id) =>
    await _dbContext.Categories.FirstOrDefaultAsync(c => c.Active && c.Id == id);
```

- Consulta de leitura: **sempre** `AsNoTracking()` + os `Include` que a resposta precisa.
  Consulta de escrita: rastreada, sem `AsNoTracking()`.
- **Todo filtro inclui `Active`.** Um `Where` sem ele devolve registro soft-deleted.
- Busca textual: `EF.Functions.ILike(coluna, $"%{termo}%")` (o `LIKE` do Postgres é
  case-sensitive). Use `IsNotEmpty()` de `Domain.Extensions` para decidir se aplica o filtro.
- Paginação devolve `PagedResult<T>`: conte com `CountAsync()` **antes** de `Skip/Take`.

## Mapeamento

Toda configuração fica em `HospitalSaoJoseDbContext.OnModelCreating` — nunca em atributos na
entidade. Para cada tabela: `ToTable`, `Id` com `ValueGeneratedNever()`, `HasMaxLength` batendo
com a migration, e índice único **filtrado** para os campos que só precisam ser únicos entre
ativos:

```csharp
entity.HasIndex(user => user.Email).IsUnique().HasFilter("\"Active\" = true");
```

Sem o filtro, desativar um usuário impediria recriar outro com o mesmo e-mail.

`[assembly: InternalsVisibleTo("WebApi.Tests")]` fica no topo de `HospitalSaoJoseDbContext.cs`,
acima do `namespace`, para os testes de integração conseguirem resolver o contexto.

## DatabaseSeeder

Roda em **todo startup** e precisa continuar idempotente. Ele sincroniza o catálogo de
`Permissions.All` com a tabela `Roles`, garante o perfil `Administrador` (`IsSystem`) com todas
as roles, cria o `Editor` só se ainda não existir, semeia as 4 categorias legadas se a tabela
estiver vazia, e cria o primeiro admin a partir de `Seed:*` apenas quando não há nenhum usuário.

Ao adicionar uma permissão nova em `Permissions`, **nada mais precisa ser feito**: o seeder cria
a role e vincula ao perfil de sistema no próximo boot.

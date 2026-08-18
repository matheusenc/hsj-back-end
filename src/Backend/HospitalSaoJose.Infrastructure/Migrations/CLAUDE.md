# Migrations

**FluentMigrator, não EF Migrations.** Não rode `dotnet ef migrations add` neste repositório —
o EF é usado só para consultar; o schema é versionado à mão aqui.

## Como adicionar uma migration

1. Acrescente uma constante em `DatabaseVersions.cs` com o próximo número:

```csharp
internal const int TABLE_DOCUMENT_TAGS = 7;
```

2. Crie `Versions/Version0000007.cs`:

```csharp
[Migration(DatabaseVersions.TABLE_DOCUMENT_TAGS, "Criando a tabela DocumentTags")]
public class Version0000007 : ForwardOnlyMigration
{
    public override void Up() { /* ... */ }
}
```

3. Espelhe as mesmas colunas e tamanhos em `OnModelCreating` do `HospitalSaoJoseDbContext`.
   As duas fontes precisam concordar — não há verificação automática.

## Regras

- `ForwardOnlyMigration` sempre: não escrevemos `Down()`. Correção é migration nova.
- Numeração é sequencial e **imutável**: nunca renumere nem edite uma migration já aplicada.
- Classes de migration são `public` (o FluentMigrator as descobre por reflexão), ao contrário do
  resto da Infrastructure.
- Índice único parcial não tem API fluente: use
  `Execute.Sql("""CREATE UNIQUE INDEX "IX_X_Y" ON "X" ("Y") WHERE "Active" = true;""")`.
  Aspas duplas são obrigatórias — o Postgres minúsculiza identificadores sem elas.
- Tipos: `AsGuid()` para ids, `AsDate()` para `DateOnly`, `AsDateTimeOffset()` para `CreatedOn`
  (vira `timestamptz`, que é o que o Npgsql espera de um `DateTime` UTC).

## Dados iniciais não vêm de migration

Roles, perfis, categorias e o primeiro admin são criados pelo `DataAccess/DatabaseSeeder.cs`,
porque dependem de `Permissions.All` (código) e do `IPasswordHasher` (DI). Não duplique isso aqui.

O runner é disparado por `DatabaseMigration.ExecuteMigrations(app.Services)` no `Program.cs`,
antes do seeder.

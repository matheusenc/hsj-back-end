# Entities

Modelo de domínio puro. Este projeto **não tem nenhum pacote NuGet** — nada de atributos de EF,
de validação ou de serialização aqui.

## Faça

- Herde de `EntityBase`: ele já dá `Id` (`Guid.CreateVersion7()`, `private set`), `Active = true`
  e `CreatedOn = DateTime.UtcNow`.
- `public sealed class`.
- Inicialize toda propriedade: `= string.Empty` para texto, `= []` para coleções.
- Navegações obrigatórias como `= null!` (o EF preenche); navegações opcionais como `?`.
- Datas de negócio (publicação, pagamento) são `DateOnly`; carimbos de sistema são `DateTime` UTC.

## Não faça

- Não anote com `[Required]`, `[MaxLength]`, `[Column]` — o mapeamento vive em
  `Infrastructure/DataAccess/HospitalSaoJoseDbContext.OnModelCreating`.
- Não coloque regra de negócio aqui; ela vive no use case.
- Não crie método de exclusão: excluir é `Active = false` feito pelo use case.

## Exemplo canônico

```csharp
public sealed class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public ICollection<Document> Documents { get; set; } = [];
}
```

## RBAC

`User` → 1 `Profile` → N `Role` (via `ProfileRole`, chave composta). `Role.Key` é a permissão
granular consumida pelos controllers; o catálogo canônico está em `../Security/Permissions.cs`.
`IsSystem = true` marca perfis e roles que o código referencia e que não podem ser
alterados/excluídos pela API.

# API Hospital São José

API REST em .NET 10 que substitui o acesso direto ao Google Firebase feito pelo site legado
(`C:\dev\hsj\public_html`). Responde por autenticação, RBAC, categorias e o armazenamento dos
documentos de transparência. Arquitetura espelhada de
[MyRecipeBook](https://github.com/welissonArley/MyRecipeBook/tree/develop).

## Layout

```
src/Backend/   HospitalSaoJose.{Api,Application,Domain,Infrastructure}
src/Shared/    HospitalSaoJose.{Communication,Exception}
tests/         CommonTestUtilities, UseCases.Tests, Validators.Tests, WebApi.Tests
```

## Regras duras de dependência

| Projeto | Pode referenciar |
|---|---|
| `Domain` | **nada** — nem projeto, nem pacote NuGet |
| `Communication` | nada |
| `Exception` | nada |
| `Application` | Communication, Exception, Domain |
| `Infrastructure` | **Domain apenas** |
| `Api` | Communication, Exception, Application, Infrastructure |

Se um código novo precisar quebrar essa tabela, o desenho está errado — mova a abstração para o
`Domain` em vez de adicionar a referência.

## Convenções que valem em todo o repositório

- `net10.0`, `Nullable` e `ImplicitUsings` habilitados; build deve ficar com **0 warnings**.
- `sealed` por padrão em classes concretas; `abstract` só em `EntityBase` e nas exceções base.
- Props sempre inicializadas (`= string.Empty`, `= []`); nunca deixar `string` não-anulável sem valor.
- **Sem multi-idioma.** Não existe `.resx`, `UseRequestLocalization` nem `Accept-Language`.
  Toda mensagem de erro é `const` em `ErrorMessages` (pt-BR).
- Soft delete em tudo: `Active = false`. Nunca `Remove`/`DELETE` físico.
- `Guid.CreateVersion7()` para ids, gerado na entidade (`ValueGeneratedNever` no EF).

Cada pasta principal tem seu próprio `CLAUDE.md` com as regras específicas — leia o da pasta
antes de criar arquivo nela.

## Rodando

```bash
dotnet build                                        # solução inteira
dotnet test tests/UseCases.Tests                    # unit
dotnet test tests/Validators.Tests                  # unit
dotnet test tests/WebApi.Tests                      # integração — exige Docker
dotnet run --project src/Backend/HospitalSaoJose.Api
```

Segredos (`ConnectionStrings:DbConnection`, `Jwt:SigningKey`, `Seed:AdminPassword`) via
variáveis de ambiente ou user-secrets. **Nunca commitar valor real em `appsettings.json`.**

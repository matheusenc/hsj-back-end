# Tests

Quatro projetos. xUnit + **Shouldly** (não FluentAssertions) + Moq + Bogus.

| Projeto | Cobre | Precisa de Docker |
|---|---|---|
| `CommonTestUtilities` | builders compartilhados (não tem teste) | não |
| `UseCases.Tests` | regras de negócio, com repositórios mockados | não |
| `Validators.Tests` | validators isolados | não |
| `WebApi.Tests` | end-to-end via `WebApplicationFactory` + Postgres em container | **sim** |

## Nomes

- Arquivo `<UseCase>Tests.cs`, na mesma árvore de pastas do código testado.
- `Success()` para o caminho feliz.
- `Execute_ShouldThrowException_When<Condição>()` nos use cases;
  `Error_When<Condição>()` nos validators e nos testes de API.
- Um `private static <UseCase> CreateUseCase(...)` no fim da classe monta as dependências.

## Builders (`CommonTestUtilities`)

Dois formatos, conforme o uso:

- **Estático** quando o mock não precisa de configuração:
  `IUnitOfWorkBuilder.Build()`, `IPasswordHasherBuilder.Build()`.
- **Fluente** quando precisa: cada método devolve `this` e o `Build()` fecha —
  `new IUserReadOnlyRepositoryBuilder().GetByEmail(user).Build()`.

Builders de mock mantêm o `I` do nome da interface que dublam (`IUserReadOnlyRepositoryBuilder`).
Os de escrita expõem o que foi persistido (`AddedUser`, `AddedDocument`, `AddedProfile`) para
o teste conferir o que o use case montou.

`PasswordHasherFake` gera `hash::<senha>` — previsível e sem o custo do Argon2.
`FileBuilder.Pdf()` devolve um stream com a assinatura `%PDF`; `FileBuilder.NotAPdf()`, sem ela.

## Colisão de nomes

Os namespaces de teste espelham as features, então `UseCases.Tests.User` esconde
`HospitalSaoJose.Domain.Entities.User`. Escreva o tipo totalmente qualificado nas assinaturas
de helper e **não** importe `HospitalSaoJose.Domain.Entities` nesses arquivos.

## Mapster

`UseCases.Tests/MapsterInitializer.cs` chama `MapsterConfiguration.Configure()` num
`[ModuleInitializer]`. Sem isso os `.Adapt<T>()` rodam sem os `Ignore` configurados e falham em
runtime. Não remova.

## Integração

`HospitalSaoJoseApplicationFactory` sobe `postgres:16-alpine` via Testcontainers e injeta
connection string, `Jwt:*`, `FileStorage:RootPath` (pasta temporária) e `Seed:*` por
configuração em memória. Migrations e seed rodam no startup da própria API — o teste exercita o
mesmo caminho da produção.

Como `WebApplicationFactory` já expõe `DisposeAsync()`, o `IAsyncLifetime` do xUnit é
implementado **explicitamente** (`async Task IAsyncLifetime.DisposeAsync()`), senão as
assinaturas conflitam.

Herde de `BaseIntegrationTest` e use `Post`/`PostFormData`/`Get`/`Delete`, que já cuidam do
header `Authorization`. `LoginAsAdmin()` devolve o token do admin semeado.

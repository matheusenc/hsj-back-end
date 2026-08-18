# Exception

Hierarquia de erro da aplicação + o catálogo de mensagens.

## Hierarquia

`HospitalSaoJoseException` (abstrata) obriga `GetStatusCode()` e `GetErrorMessages()`.
O `ExceptionFilter` da Api usa esses dois métodos e nada mais — qualquer outra exceção vira
500 com `ErrorMessages.UNKNOWN_ERROR`.

| Exceção | Status | Quando |
|---|---|---|
| `ErrorOnValidationException` | 400 | falha de validator ou de regra de negócio verificável |
| `InvalidLoginException` | 401 | e-mail/senha inválidos (mensagem fixa, nunca detalhe qual dos dois) |
| `ForbiddenAccessException` | 403 | perfil/role `IsSystem`, auto-desativação |
| `NotFoundException` | 404 | id inexistente ou já desativado |

Como o namespace do projeto é `HospitalSaoJose.Exception`, a base precisa herdar de
`System.Exception` totalmente qualificado.

## ErrorMessages

**Toda** string voltada ao usuário mora em `ErrorMessages.cs`, como `public const string` em
pt-BR, com chave `SCREAMING_SNAKE_CASE` prefixada pelo assunto
(`VALIDATION_*`, `<ENTIDADE>_NOT_FOUND`, `UNKNOWN_ERROR`).

Antes de adicionar uma chave, **procure uma existente que sirva** — mensagens quase iguais com
nomes diferentes é o problema que este arquivo existe para evitar.

Literal de erro em validator, use case, controller ou `Program.cs` é bug: aponte para a `const`.

## Sem multi-idioma

Este projeto **não** usa `.resx`, `ResourceManager`, `CultureInfo` nem `Accept-Language`. Se
algum dia precisar, a troca é substituir `ErrorMessages` por resx — nenhum outro arquivo muda,
porque todos referenciam as chaves e não os textos.

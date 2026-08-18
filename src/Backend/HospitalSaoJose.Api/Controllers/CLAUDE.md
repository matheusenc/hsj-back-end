# Controllers

Camada fina: recebe, delega para o use case, devolve. **Zero regra de negócio, zero `try/catch`** —
erros sobem e viram resposta no `Filters/ExceptionFilter.cs`.

## Forma

```csharp
[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
```

Não existe base controller. Rotas são geradas em minúsculas
(`AddRouting(options => options.LowercaseUrls = true)`) e literais extras vão em kebab-case
(`[HttpPut("change-password")]`, `[HttpGet("{id}/download")]`).

## Regras

- O use case é injetado **por action**, via `[FromServices]`, nunca pelo construtor.
- Ordem dos parâmetros: `[FromRoute]` (quando houver) → `[FromServices]` → `[FromBody]`/`[FromQuery]`/`[FromForm]`.
- Retorno sempre `Task<IActionResult>`: `Created(string.Empty, response)` / `Ok(response)` /
  `NoContent()` / `File(...)`.
- `[ProducesResponseType]` no caminho feliz **e em cada erro que a action pode produzir**
  (`typeof(ResponseErrorJson)` para 400/401/403/404). 204 usa a sobrecarga sem `typeof`.
- Proteção com `[HasPermission(Permissions.X)]`; `[Authorize]` puro só quando basta estar
  logado (`users/me`, `change-password`). Endpoint público não leva atributo nenhum —
  `GET /categories`, `GET /documents` e `GET /documents/{id}/download` são lidos pelo site.

## Upload

O controller é o único lugar que conhece `IFormFile`. Converta para o DTO neutro antes de
chamar o use case:

```csharp
private static DocumentFile? DocumentFileFrom(IFormFile? file) =>
    file is null || file.Length == 0
        ? null
        : new DocumentFile(file.OpenReadStream(), Path.GetFileName(file.FileName), file.Length);
```

`Path.GetFileName` é obrigatório: o nome vem do cliente e pode carregar caminho.

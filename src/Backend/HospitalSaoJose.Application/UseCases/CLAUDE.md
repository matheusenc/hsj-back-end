# Use Cases

Uma pasta por feature. **Não existe `IUseCase<TIn,TOut>` genérico** — cada use case tem sua
própria interface, no mesmo diretório da implementação.

```
UseCases/<Agregado>/<Ação>/
    I<Nome>UseCase.cs
    <Nome>UseCase.cs
    <Nome>Validator.cs      (quando a validação é específica desta ação)
UseCases/<Agregado>/<Agregado>Validator.cs   (quando Register e Update compartilham o request)
```

## Regras

- O método é **sempre `Execute`**. Nunca `ExecuteAsync`, nunca `DoAsync`, nunca dois métodos
  públicos na mesma classe.
- Dependências por construtor, campos `private readonly _camelCase`.
- A validação roda no início do `Execute`, num
  `private async Task ValidateAndThrowOnFailures(...)` que instancia o validator com `new`.
- Checagens que dependem do banco (e-mail duplicado, categoria inexistente) **não** vão no
  validator: são anexadas ao mesmo `ValidationResult` como
  `result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.X))`, para o usuário
  receber todos os erros de uma vez.
- Ao final: `throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList())`.
- "Não encontrado" é `NotFoundException`; violação de regra de sistema (`IsSystem`, auto-exclusão)
  é `ForbiddenAccessException`. Nunca retorne `null` para sinalizar erro.
- Só use cases de escrita chamam `IUnitOfWork.Commit()`.
- Mapeamento com Mapster (`.Adapt<T>()`); configuração central em `../Mappings/MapsterConfiguration.cs`.
  Se um `Ignore` novo for preciso, ele vai lá — não dê `new` manual na entidade só para escapar.

## Colisão de nomes

O namespace `...UseCases.User` esconde `HospitalSaoJose.Domain.Entities.User`. Dentro destas
pastas escreva `Domain.Entities.User`, `Domain.Entities.Profile`, etc.

## Arquivo, não `IFormFile`

A Application não conhece ASP.NET. Upload chega como
`Document/DocumentFile.cs` (`Stream` + nome + tamanho) e o download sai como `DocumentDownload`.
A conversão a partir de `IFormFile` é responsabilidade do controller.

## Exemplo canônico

```csharp
public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
{
    await ValidateAndThrowOnFailures(request);

    var user = request.Adapt<Domain.Entities.User>();
    user.Password = _passwordHasher.HashPassword(request.Password);

    await _userWriteOnlyRepository.Add(user);
    await _unitOfWork.Commit();

    return new ResponseRegisteredUserJson { Id = user.Id, Name = user.Name, Email = user.Email };
}
```

## Validators

- `AbstractValidator<Request...Json>` — sempre sobre o DTO de Communication, **nunca** sobre a entidade.
- `Cascade(CascadeMode.Stop)` em toda regra encadeada, para não duplicar mensagem.
- Mensagens vêm de `ErrorMessages`; string literal aqui é bug.
- Regras reaproveitáveis viram `extension` block em `Shared/Validators/`
  (`.Password()`, `.Page()`, `.PageSize()`).

## Ao registrar um use case novo

Adicione a linha `AddScoped` em `../DependencyInjectionExtension.cs` — o registro é explícito,
não há varredura por assembly.

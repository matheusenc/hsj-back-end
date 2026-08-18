# Communication

O contrato HTTP. Projeto sem nenhuma referência — só DTOs.

## Layout

Duas pastas **planas**, sem subpastas por agregado:

```
Requests/    Request<Verbo><Substantivo>Json.cs
Responses/   Response<Substantivo>Json.cs
```

## Regras

- `public sealed class`, props `{ get; set; }` públicas, todas inicializadas
  (`= string.Empty`, `= []`, `= new()`).
- Sufixo `Json` é obrigatório no nome do arquivo e da classe.
- Um request compartilhado entre criar e editar não ganha nome duplicado:
  `RequestCategoryJson` e `RequestDocumentJson` servem ao `Register` e ao `Update`. Só crie
  `RequestRegister...` / `RequestUpdate...` separados quando os campos realmente divergirem
  (é o caso de `Role`, cuja `Key` é imutável após a criação).
- Nunca exponha entidade de domínio nem `Password` numa response.
- Listas paginadas trazem `Page`, `PageSize`, `TotalCount` e `TotalPages` ao lado do array —
  a tela de transparência precisa habilitar/desabilitar "Anterior/Próximo".
- `ResponseErrorJson` é o corpo de **todo** erro da API; `AccessTokenExpired` distingue token
  vencido de acesso negado, para o front decidir entre relogar e mostrar mensagem.
- `DownloadUrl` é **relativo** (`/documents/{id}/download`). Quem monta a URL absoluta é o
  cliente, com a base da API.

## Não faça

- Não use `record` (a serialização e o binding de `[FromForm]` ficam mais previsíveis com classe).
- Não coloque atributo de validação: quem valida é o FluentValidation, na Application.
- Não referencie `Domain` — isso quebraria o isolamento do contrato.

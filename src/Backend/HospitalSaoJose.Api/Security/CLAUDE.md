# Security (autorização por permissão)

O RBAC tem duas metades: as tabelas (`Profile`, `Role`, `ProfileRole`) e o catálogo em código
(`Domain/Security/Permissions.cs`). Esta pasta liga uma coisa na outra.

## Como funciona

1. No login, `JwtTokenHandler` emite uma claim `permission` por `Role.Key` do perfil do usuário,
   mais `superadmin` quando o perfil é `IsSystem`.
2. `[HasPermission("documents:create")]` vira a policy `permission:documents:create`.
3. `PermissionAuthorizationPolicyProvider` cria essa policy sob demanda — por isso **não é
   preciso registrar `AddPolicy` para cada permissão** no `Program.cs`.
4. `PermissionAuthorizationHandler` aprova se o usuário tem a claim `superadmin` **ou** a claim
   `permission` exigida.

## Regras

- Toda rota protegida usa `[HasPermission(Permissions.X)]`, sempre com a constante — **nunca**
  string literal e **nunca** `[Authorize(Roles = "...")]`.
- Permissão nova = uma `const` em `Permissions` **e** uma entrada em `Permissions.All`. O
  `DatabaseSeeder` cria a `Role` e vincula ao perfil `Administrador` no próximo boot; não
  escreva migration para isso.
- Excluir uma role **fecha** o endpoint (ninguém mais tem a claim), não o abre. A saída de
  emergência é o `superadmin`, e é por isso que o perfil `Administrador` é `IsSystem` e não pode
  ser apagado pela API.

## Consequência aceita

As permissões viajam dentro do token. Trocar o perfil de alguém só faz efeito no próximo login —
janela de até `Jwt:ExpirationTimeMinutes` (60). Se um dia isso não bastar, o caminho é um
`SecurityStamp` no `User` comparado no `OnTokenValidated`, não trocar o modelo de claims.

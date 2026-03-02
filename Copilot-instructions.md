# Copilot Instructions - Manager System

## Arquitetura e Organização
- Estruture o backend por módulos (`Auth`, `Users`, `Projects`, `Tasks`, `Comments`) com **Application Pattern** e **Service Pattern**.
- Priorize métodos curtos (aprox. até 25 linhas) e extraia lógica para métodos privados quando necessário.
- Use DTOs para entrada e saída, evitando expor entidades diretamente.
- Padronize retornos com `BaseResponse<T>` contendo `data`, `errors` e `hasErrors`.

## Backend (.NET / ASP.NET Core / EF Core)
- Usar ASP.NET Core Identity como base de usuários e perfis de acesso.
- Implementar autenticação com JWT (`accessToken` + `refreshToken`).
- Usar `EntityFrameworkCore` com `Npgsql` e migrations para PostgreSQL.
- Ativar validações básicas de entrada e retornar mensagens claras de erro.
- Toda alteração em tarefas deve gerar registro em `TaskHistory`.
- Preferir consultas assíncronas e projeções com `Select` para reduzir payload.

## Segurança
- Nunca salvar segredos em texto puro.
- Definir tempos de expiração para tokens.
- Validar ownership/autorização em operações de atualização e exclusão.
- Aplicar `RequireAuthorization()` em endpoints protegidos.

## Frontend (Nuxt + Nuxt UI)
- Criar páginas dedicadas (não SPA única): Dashboard, Projetos, Tarefas/Kanban, Calendário e Configurações.
- Manter sidebar fixa com navegação rápida.
- Usar componentes do Nuxt UI para consistência visual e acessibilidade.
- Em Kanban: permitir drag-and-drop, filtros, criação de colunas, criação/edição rápida por modal e visualização de histórico.

## Qualidade
- Adicionar testes focados para regras de negócio críticas.
- Manter alterações pequenas e cirúrgicas.
- Validar build e testes após cada bloco relevante de mudanças.

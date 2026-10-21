# Biblioteca API

API RESTful para controlar o acervo de uma pequena biblioteca. Permite cadastrar livros, consultar os títulos disponíveis, atualizar seus dados e excluir registros. É destinada a quem precisa organizar o acervo e acompanhar a disponibilidade dos livros.

## Integrante

Luiz Henrique Dos Reis Grabe — RM 556001.

Disciplina: C# Software Development — CP6. Trabalho individual.

## Tecnologias

- C# e .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core 10
- SQLite
- Swagger / OpenAPI

O banco é o arquivo `biblioteca.db`, criado localmente pelas migrations. Não é necessário instalar um servidor de banco de dados.

## Executar localmente

Pré-requisito: SDK do .NET 10 instalado.

No terminal, entre na pasta que contém `BibliotecaApi.csproj` e execute:

```bash
dotnet restore
dotnet tool restore
dotnet ef database update
dotnet run
```

A migration inicial já está incluída no projeto. O comando `dotnet ef database update` cria o banco e aplica as migrations pendentes.

Abra o Swagger: http://localhost:5050/swagger

Documento OpenAPI: http://localhost:5050/swagger/v1/swagger.json

## Endpoints

| Método | Rota | Operação | Respostas |
| --- | --- | --- | --- |
| GET | `/livros` | Listar todos os livros | 200 |
| GET | `/livros/{id}` | Consultar um livro | 200, 404 |
| POST | `/livros` | Cadastrar um livro | 201, 400 |
| PUT | `/livros/{id}` | Atualizar todos os dados do livro | 200, 400, 404 |
| DELETE | `/livros/{id}` | Excluir um livro | 204, 404 |

Exemplo de corpo para POST e PUT:

```json
{
  "titulo": "Dom Casmurro",
  "autor": "Machado de Assis",
  "anoPublicacao": 1899,
  "disponivel": true
}
```

O ID é gerado pelo banco. Título e autor são obrigatórios, com limites de 200 e 150 caracteres. O ano deve estar entre 1 e o ano atual. Dados inválidos retornam 400 com os erros de validação. Um ID inexistente retorna 404. O POST retorna 201 e o endereço do registro no cabeçalho `Location`; o DELETE retorna 204, sem corpo.

Todas as consultas e gravações no banco usam `async` e `await`. Os endpoints são definidos com `MapGet`, `MapPost`, `MapPut` e `MapDelete`, sem Controllers.

## Evidências

As capturas dos testes no Swagger estão na pasta [evidencias](evidencias). Para repetir os testes, execute o POST, consulte a lista e o ID retornado, faça o PUT e finalize com o DELETE. Também é possível testar um ID inexistente e enviar um título vazio para verificar os erros.

| Teste | Evidência |
| --- | --- |
| POST — 201 | [Cadastro](evidencias/01-post-livros.jpg) |
| GET — 200 | [Listagem](evidencias/02-get-livros.jpg) |
| GET por ID — 200 | [Consulta](evidencias/03-get-livro-id.jpg) |
| PUT — 200 | [Atualização](evidencias/04-put-livro.jpg) |
| DELETE — 204 | [Exclusão](evidencias/05-delete-livro.jpg) |
| GET após exclusão — 404 | [Registro inexistente](evidencias/06-get-inexistente.jpg) |
| POST inválido — 400 | [Validação](evidencias/07-post-invalido.jpg) |

## Referência

[Tutorial de Minimal API da Microsoft](https://learn.microsoft.com/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0).

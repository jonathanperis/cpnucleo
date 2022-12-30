using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cpnucleo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "Recursos",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Login", "Nome", "Salt", "Senha" },
                values: new object[] { new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "usuario.teste", "Recurso de teste", "okVTEMBEAbjnjKmD3On1qKwDT0+vfBRAzDM/T7vHqH+gZJxV8/9rRhqiALLlLC7r", "k8n3YJ7em+uo32BbpRNgjB+kX6uRCJLN7V1L4Q7WwUqDrpz00uCHi+wOLJBZJkOQ" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Sistemas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Descricao", "Nome" },
                values: new object[] { new Guid("b865f5ca-d3c2-46ff-96c8-860207b563c9"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Descrição do sistema de teste", "Sistema de teste" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "TiposTarefas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Image", "Nome" },
                values: new object[,]
                {
                    { new Guid("6f1d2369-2879-444c-b50f-1c022f7a40b1"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "../gif/gif-problema.gif", "Problema" },
                    { new Guid("9ecea6c5-dced-44a8-b4cf-9dfcf333fb0a"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "../gif/gif-projeto.gif", "Projeto" },
                    { new Guid("a149c773-879d-4b93-905c-2a02835775c1"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "../gif/gif-melhoria.gif", "Melhoria" },
                    { new Guid("eb94316e-a987-4809-9b9d-eb88051f054c"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "../gif/gif-requisicao.gif", "Requisição" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Workflows",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Nome", "Ordem" },
                values: new object[,]
                {
                    { new Guid("0c17bf58-c14b-44de-8883-e7616bf29134"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Análise", 1 },
                    { new Guid("4562c0b8-d476-46eb-a58d-1ff3a86266ac"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Desenvolvimento", 2 },
                    { new Guid("bef7c738-0396-4fe0-af66-a4629261fc8e"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teste", 3 },
                    { new Guid("d3af39ba-e690-47e5-a40d-0d849e07a294"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finalizado", 4 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Projetos",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "IdSistema", "Nome" },
                values: new object[] { new Guid("38e2dce9-9f48-486a-88c1-12985c3b72ef"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("b865f5ca-d3c2-46ff-96c8-860207b563c9"), "Projeto de teste" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Tarefas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "DataInicio", "DataTermino", "Detalhe", "IdProjeto", "IdRecurso", "IdTipoTarefa", "IdWorkflow", "Nome", "QtdHoras" },
                values: new object[] { new Guid("82fe661a-620b-40b5-980a-4104b03ce873"), true, null, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("38e2dce9-9f48-486a-88c1-12985c3b72ef"), new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), new Guid("9ecea6c5-dced-44a8-b4cf-9dfcf333fb0a"), new Guid("0c17bf58-c14b-44de-8883-e7616bf29134"), "Sistema de teste", 40 });

            migrationBuilder.InsertData(
                schema: "public",
                table: "RecursosTarefas",
                columns: new[] { "Id", "Ativo", "DataExclusao", "DataInclusao", "IdRecurso", "IdTarefa", "PercentualTarefa" },
                values: new object[] { new Guid("c38956cb-3f2a-4934-a042-2a8bccdcb2ed"), true, null, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), new Guid("82fe661a-620b-40b5-980a-4104b03ce873"), 25 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "RecursosTarefas",
                keyColumn: "Id",
                keyValue: new Guid("c38956cb-3f2a-4934-a042-2a8bccdcb2ed"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "TiposTarefas",
                keyColumn: "Id",
                keyValue: new Guid("6f1d2369-2879-444c-b50f-1c022f7a40b1"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "TiposTarefas",
                keyColumn: "Id",
                keyValue: new Guid("a149c773-879d-4b93-905c-2a02835775c1"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "TiposTarefas",
                keyColumn: "Id",
                keyValue: new Guid("eb94316e-a987-4809-9b9d-eb88051f054c"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Workflows",
                keyColumn: "Id",
                keyValue: new Guid("4562c0b8-d476-46eb-a58d-1ff3a86266ac"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Workflows",
                keyColumn: "Id",
                keyValue: new Guid("bef7c738-0396-4fe0-af66-a4629261fc8e"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Workflows",
                keyColumn: "Id",
                keyValue: new Guid("d3af39ba-e690-47e5-a40d-0d849e07a294"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tarefas",
                keyColumn: "Id",
                keyValue: new Guid("82fe661a-620b-40b5-980a-4104b03ce873"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Projetos",
                keyColumn: "Id",
                keyValue: new Guid("38e2dce9-9f48-486a-88c1-12985c3b72ef"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Recursos",
                keyColumn: "Id",
                keyValue: new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "TiposTarefas",
                keyColumn: "Id",
                keyValue: new Guid("9ecea6c5-dced-44a8-b4cf-9dfcf333fb0a"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Workflows",
                keyColumn: "Id",
                keyValue: new Guid("0c17bf58-c14b-44de-8883-e7616bf29134"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Sistemas",
                keyColumn: "Id",
                keyValue: new Guid("b865f5ca-d3c2-46ff-96c8-860207b563c9"));
        }
    }
}

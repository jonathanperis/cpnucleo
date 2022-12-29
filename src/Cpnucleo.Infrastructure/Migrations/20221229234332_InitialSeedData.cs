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
                values: new object[] { new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9222), "usuario.teste", "Recurso de teste", "okVTEMBEAbjnjKmD3On1qKwDT0+vfBRAzDM/T7vHqH+gZJxV8/9rRhqiALLlLC7r", "k8n3YJ7em+uo32BbpRNgjB+kX6uRCJLN7V1L4Q7WwUqDrpz00uCHi+wOLJBZJkOQ" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Sistemas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Descricao", "Nome" },
                values: new object[] { new Guid("b865f5ca-d3c2-46ff-96c8-860207b563c9"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9117), "Descrição do sistema de teste", "Sistema de teste" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "TiposTarefas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Image", "Nome" },
                values: new object[] { new Guid("91d6a672-22dc-4b90-8d7f-0533d2150c44"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9232), "success.png", "TipoTarefa de teste" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Workflows",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "Nome", "Ordem" },
                values: new object[,]
                {
                    { new Guid("0c17bf58-c14b-44de-8883-e7616bf29134"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9198), "Análise", 1 },
                    { new Guid("4562c0b8-d476-46eb-a58d-1ff3a86266ac"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9203), "Desenvolvimento", 2 },
                    { new Guid("bef7c738-0396-4fe0-af66-a4629261fc8e"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9205), "Teste", 3 },
                    { new Guid("d3af39ba-e690-47e5-a40d-0d849e07a294"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9208), "Finalizado", 4 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Projetos",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "IdSistema", "Nome" },
                values: new object[] { new Guid("38e2dce9-9f48-486a-88c1-12985c3b72ef"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9185), new Guid("b865f5ca-d3c2-46ff-96c8-860207b563c9"), "Projeto de teste" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Tarefas",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataExclusao", "DataInclusao", "DataInicio", "DataTermino", "Detalhe", "IdProjeto", "IdRecurso", "IdTipoTarefa", "IdWorkflow", "Nome", "QtdHoras" },
                values: new object[] { new Guid("82fe661a-620b-40b5-980a-4104b03ce873"), true, null, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9315), new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9278), new DateTime(2023, 1, 3, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9279), null, new Guid("38e2dce9-9f48-486a-88c1-12985c3b72ef"), new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), new Guid("91d6a672-22dc-4b90-8d7f-0533d2150c44"), new Guid("0c17bf58-c14b-44de-8883-e7616bf29134"), "Sistema de teste", 40 });

            migrationBuilder.InsertData(
                schema: "public",
                table: "RecursosTarefas",
                columns: new[] { "Id", "Ativo", "DataExclusao", "DataInclusao", "IdRecurso", "IdTarefa", "PercentualTarefa" },
                values: new object[] { new Guid("c38956cb-3f2a-4934-a042-2a8bccdcb2ed"), true, null, new DateTime(2022, 12, 29, 23, 43, 31, 667, DateTimeKind.Utc).AddTicks(9327), new Guid("ae9cab55-01f8-4bd1-8ca0-92f174bb1aa0"), new Guid("82fe661a-620b-40b5-980a-4104b03ce873"), 25 });
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
                keyValue: new Guid("91d6a672-22dc-4b90-8d7f-0533d2150c44"));

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

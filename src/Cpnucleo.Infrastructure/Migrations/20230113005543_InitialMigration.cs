using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cpnucleo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Impedimentos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impedimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Senha = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Salt = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sistemas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposTarefas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTarefas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IdSistema = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_Sistemas_IdSistema",
                        column: x => x.IdSistema,
                        principalSchema: "public",
                        principalTable: "Sistemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecursosProjetos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRecurso = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProjeto = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosProjetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosProjetos_Projetos_IdProjeto",
                        column: x => x.IdProjeto,
                        principalSchema: "public",
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecursosProjetos_Recursos_IdRecurso",
                        column: x => x.IdRecurso,
                        principalSchema: "public",
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QtdHoras = table.Column<int>(type: "integer", nullable: false),
                    Detalhe = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IdProjeto = table.Column<Guid>(type: "uuid", nullable: false),
                    IdWorkflow = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRecurso = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTipoTarefa = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Projetos_IdProjeto",
                        column: x => x.IdProjeto,
                        principalSchema: "public",
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_Recursos_IdRecurso",
                        column: x => x.IdRecurso,
                        principalSchema: "public",
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_TiposTarefas_IdTipoTarefa",
                        column: x => x.IdTipoTarefa,
                        principalSchema: "public",
                        principalTable: "TiposTarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_Workflows_IdWorkflow",
                        column: x => x.IdWorkflow,
                        principalSchema: "public",
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apontamentos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DataApontamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QtdHoras = table.Column<int>(type: "integer", nullable: false),
                    IdTarefa = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRecurso = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apontamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apontamentos_Tarefas_IdTarefa",
                        column: x => x.IdTarefa,
                        principalSchema: "public",
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImpedimentosTarefas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IdTarefa = table.Column<Guid>(type: "uuid", nullable: false),
                    IdImpedimento = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpedimentosTarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpedimentosTarefas_Impedimentos_IdImpedimento",
                        column: x => x.IdImpedimento,
                        principalSchema: "public",
                        principalTable: "Impedimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImpedimentosTarefas_Tarefas_IdTarefa",
                        column: x => x.IdTarefa,
                        principalSchema: "public",
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecursosTarefas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PercentualTarefa = table.Column<int>(type: "integer", nullable: false),
                    IdRecurso = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTarefa = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusteredKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosTarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosTarefas_Recursos_IdRecurso",
                        column: x => x.IdRecurso,
                        principalSchema: "public",
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecursosTarefas_Tarefas_IdTarefa",
                        column: x => x.IdTarefa,
                        principalSchema: "public",
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Apontamentos_ClusteredKey",
                schema: "public",
                table: "Apontamentos",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_Apontamentos_IdTarefa",
                schema: "public",
                table: "Apontamentos",
                column: "IdTarefa");

            migrationBuilder.CreateIndex(
                name: "IX_Impedimentos_ClusteredKey",
                schema: "public",
                table: "Impedimentos",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_ImpedimentosTarefas_ClusteredKey",
                schema: "public",
                table: "ImpedimentosTarefas",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_ImpedimentosTarefas_IdImpedimento",
                schema: "public",
                table: "ImpedimentosTarefas",
                column: "IdImpedimento");

            migrationBuilder.CreateIndex(
                name: "IX_ImpedimentosTarefas_IdTarefa",
                schema: "public",
                table: "ImpedimentosTarefas",
                column: "IdTarefa");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_ClusteredKey",
                schema: "public",
                table: "Projetos",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_IdSistema",
                schema: "public",
                table: "Projetos",
                column: "IdSistema");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_ClusteredKey",
                schema: "public",
                table: "Recursos",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosProjetos_ClusteredKey",
                schema: "public",
                table: "RecursosProjetos",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosProjetos_IdProjeto",
                schema: "public",
                table: "RecursosProjetos",
                column: "IdProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosProjetos_IdRecurso",
                schema: "public",
                table: "RecursosProjetos",
                column: "IdRecurso");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosTarefas_ClusteredKey",
                schema: "public",
                table: "RecursosTarefas",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosTarefas_IdRecurso",
                schema: "public",
                table: "RecursosTarefas",
                column: "IdRecurso");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosTarefas_IdTarefa",
                schema: "public",
                table: "RecursosTarefas",
                column: "IdTarefa");

            migrationBuilder.CreateIndex(
                name: "IX_Sistemas_ClusteredKey",
                schema: "public",
                table: "Sistemas",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_ClusteredKey",
                schema: "public",
                table: "Tarefas",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdProjeto",
                schema: "public",
                table: "Tarefas",
                column: "IdProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdRecurso",
                schema: "public",
                table: "Tarefas",
                column: "IdRecurso");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdTipoTarefa",
                schema: "public",
                table: "Tarefas",
                column: "IdTipoTarefa");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdWorkflow",
                schema: "public",
                table: "Tarefas",
                column: "IdWorkflow");

            migrationBuilder.CreateIndex(
                name: "IX_TiposTarefas_ClusteredKey",
                schema: "public",
                table: "TiposTarefas",
                column: "ClusteredKey");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_ClusteredKey",
                schema: "public",
                table: "Workflows",
                column: "ClusteredKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apontamentos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ImpedimentosTarefas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RecursosProjetos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RecursosTarefas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Impedimentos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Tarefas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Projetos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Recursos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TiposTarefas",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Workflows",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Sistemas",
                schema: "public");
        }
    }
}

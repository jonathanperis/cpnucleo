using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cpnucleo.Infra.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CPN_TB_IMPEDIMENTO",
                columns: table => new
                {
                    IMP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IMP_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IMP_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    IMP_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    IMP_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    IMP_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_IMPEDIMENTO", x => x.IMP_ID);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_RECURSO",
                columns: table => new
                {
                    REC_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    REC_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    REC_LOGIN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    REC_SENHA = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    REC_SENHA_SALT = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    REC_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    REC_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    REC_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    REC_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_RECURSO", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_SISTEMA",
                columns: table => new
                {
                    SIS_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SIS_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SIS_DESCRICAO = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    SIS_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    SIS_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    SIS_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    SIS_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_SISTEMA", x => x.SIS_ID);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_TIPO_TAREFA",
                columns: table => new
                {
                    TIP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TIP_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TIP_IMAGE_CARD = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TIP_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    TIP_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    TIP_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    TIP_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_TIPO_TAREFA", x => x.TIP_ID);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_WORKFLOW",
                columns: table => new
                {
                    WOR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WOR_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WOR_ORDEM = table.Column<int>(type: "int", nullable: false),
                    WOR_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    WOR_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    WOR_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    WOR_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_WORKFLOW", x => x.WOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_PROJETO",
                columns: table => new
                {
                    PROJ_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PROJ_NOME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SIS_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PROJ_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    PROJ_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    PROJ_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    PROJ_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_PROJETO", x => x.PROJ_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_PROJETO_CPN_TB_SISTEMA_SIS_ID",
                        column: x => x.SIS_ID,
                        principalTable: "CPN_TB_SISTEMA",
                        principalColumn: "SIS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_RECURSO_PROJETO",
                columns: table => new
                {
                    RPROJ_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    REC_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PROJ_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RPROJ_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    RPROJ_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    RPROJ_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_RECURSO_PROJETO", x => x.RPROJ_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_RECURSO_PROJETO_CPN_TB_PROJETO_PROJ_ID",
                        column: x => x.PROJ_ID,
                        principalTable: "CPN_TB_PROJETO",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_RECURSO_PROJETO_CPN_TB_RECURSO_REC_ID",
                        column: x => x.REC_ID,
                        principalTable: "CPN_TB_RECURSO",
                        principalColumn: "REC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_TAREFA",
                columns: table => new
                {
                    TAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TAR_NOME = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    TAR_DATA_INICIO = table.Column<DateTime>(type: "datetime", nullable: false),
                    TAR_DATA_TERMINO = table.Column<DateTime>(type: "datetime", nullable: false),
                    TAR_QTD_HORAS = table.Column<int>(type: "int", nullable: false),
                    TAR_DETALHE = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    PROJ_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WOR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    REC_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TIP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TAR_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    TAR_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    TAR_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    TAR_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_TAREFA", x => x.TAR_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_CPN_TB_PROJETO_PROJ_ID",
                        column: x => x.PROJ_ID,
                        principalTable: "CPN_TB_PROJETO",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_CPN_TB_RECURSO_REC_ID",
                        column: x => x.REC_ID,
                        principalTable: "CPN_TB_RECURSO",
                        principalColumn: "REC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_CPN_TB_TIPO_TAREFA_TIP_ID",
                        column: x => x.TIP_ID,
                        principalTable: "CPN_TB_TIPO_TAREFA",
                        principalColumn: "TIP_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_CPN_TB_WORKFLOW_WOR_ID",
                        column: x => x.WOR_ID,
                        principalTable: "CPN_TB_WORKFLOW",
                        principalColumn: "WOR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_APONTAMENTO",
                columns: table => new
                {
                    APT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APT_DESCRICAO = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    APT_DATA_LANCAMENTO = table.Column<DateTime>(type: "datetime", nullable: false),
                    APT_QTD_HORAS = table.Column<int>(type: "int", nullable: false),
                    TAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    REC_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APT_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    APT_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    APT_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    APT_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_APONTAMENTO", x => x.APT_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_APONTAMENTO_CPN_TB_TAREFA_TAR_ID",
                        column: x => x.TAR_ID,
                        principalTable: "CPN_TB_TAREFA",
                        principalColumn: "TAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_RECURSO_TAREFA",
                columns: table => new
                {
                    RTAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RTAR_PERCENTUAL = table.Column<int>(type: "int", nullable: false),
                    REC_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RTAR_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    RTAR_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    RTAR_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    RTAR_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_RECURSO_TAREFA", x => x.RTAR_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_RECURSO_TAREFA_CPN_TB_RECURSO_REC_ID",
                        column: x => x.REC_ID,
                        principalTable: "CPN_TB_RECURSO",
                        principalColumn: "REC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_RECURSO_TAREFA_CPN_TB_TAREFA_TAR_ID",
                        column: x => x.TAR_ID,
                        principalTable: "CPN_TB_TAREFA",
                        principalColumn: "TAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CPN_TB_TAREFA_IMPEDIMENTO",
                columns: table => new
                {
                    ITAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ITAR_DESCRICAO = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    TAR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IMP_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ITAR_DATA_INCLUSAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    ITAR_DATA_ALTERACAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    ITAR_DATA_EXCLUSAO = table.Column<DateTime>(type: "datetime", nullable: true),
                    ITAR_ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPN_TB_TAREFA_IMPEDIMENTO", x => x.ITAR_ID);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_IMPEDIMENTO_CPN_TB_IMPEDIMENTO_IMP_ID",
                        column: x => x.IMP_ID,
                        principalTable: "CPN_TB_IMPEDIMENTO",
                        principalColumn: "IMP_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPN_TB_TAREFA_IMPEDIMENTO_CPN_TB_TAREFA_TAR_ID",
                        column: x => x.TAR_ID,
                        principalTable: "CPN_TB_TAREFA",
                        principalColumn: "TAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CPN_TB_RECURSO",
                columns: new[] { "REC_ID", "REC_ATIVO", "REC_DATA_ALTERACAO", "REC_DATA_EXCLUSAO", "REC_DATA_INCLUSAO", "REC_LOGIN", "REC_NOME", "REC_SENHA_SALT", "REC_SENHA" },
                values: new object[] { new Guid("78ad39f8-fc09-454f-a777-350a42a36358"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7055), "usuario.teste", "Recurso de teste", "okVTEMBEAbjnjKmD3On1qKwDT0+vfBRAzDM/T7vHqH+gZJxV8/9rRhqiALLlLC7r", "k8n3YJ7em+uo32BbpRNgjB+kX6uRCJLN7V1L4Q7WwUqDrpz00uCHi+wOLJBZJkOQ" });

            migrationBuilder.InsertData(
                table: "CPN_TB_SISTEMA",
                columns: new[] { "SIS_ID", "SIS_ATIVO", "SIS_DATA_ALTERACAO", "SIS_DATA_EXCLUSAO", "SIS_DATA_INCLUSAO", "SIS_DESCRICAO", "SIS_NOME" },
                values: new object[] { new Guid("acc21f00-de8d-49e8-9df1-ea74a0148df2"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7011), "Descrição do sistema de teste", "Sistema de teste" });

            migrationBuilder.InsertData(
                table: "CPN_TB_TIPO_TAREFA",
                columns: new[] { "TIP_ID", "TIP_ATIVO", "TIP_DATA_ALTERACAO", "TIP_DATA_EXCLUSAO", "TIP_DATA_INCLUSAO", "TIP_IMAGE_CARD", "TIP_NOME" },
                values: new object[] { new Guid("bfbfa056-933c-4419-b2d5-101a3e2fdc33"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7060), "success.png", "TipoTarefa de teste" });

            migrationBuilder.InsertData(
                table: "CPN_TB_WORKFLOW",
                columns: new[] { "WOR_ID", "WOR_ATIVO", "WOR_DATA_ALTERACAO", "WOR_DATA_EXCLUSAO", "WOR_DATA_INCLUSAO", "WOR_NOME", "WOR_ORDEM" },
                values: new object[] { new Guid("4896727a-1768-4f4a-98a0-802589f055cc"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7042), "Finalizado", 4 });

            migrationBuilder.InsertData(
                table: "CPN_TB_WORKFLOW",
                columns: new[] { "WOR_ID", "WOR_ATIVO", "WOR_DATA_ALTERACAO", "WOR_DATA_EXCLUSAO", "WOR_DATA_INCLUSAO", "WOR_NOME", "WOR_ORDEM" },
                values: new object[] { new Guid("8be601ef-8c1c-40b2-8d99-cdf3594d4a70"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7040), "Desenvolvimento", 2 });

            migrationBuilder.InsertData(
                table: "CPN_TB_WORKFLOW",
                columns: new[] { "WOR_ID", "WOR_ATIVO", "WOR_DATA_ALTERACAO", "WOR_DATA_EXCLUSAO", "WOR_DATA_INCLUSAO", "WOR_NOME", "WOR_ORDEM" },
                values: new object[] { new Guid("ac5e6309-16a7-4e37-9bd0-541d9f3cea0b"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7038), "Análise", 1 });

            migrationBuilder.InsertData(
                table: "CPN_TB_WORKFLOW",
                columns: new[] { "WOR_ID", "WOR_ATIVO", "WOR_DATA_ALTERACAO", "WOR_DATA_EXCLUSAO", "WOR_DATA_INCLUSAO", "WOR_NOME", "WOR_ORDEM" },
                values: new object[] { new Guid("e395dd77-4733-462a-a5fb-346a86ed5127"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7041), "Teste", 3 });

            migrationBuilder.InsertData(
                table: "CPN_TB_PROJETO",
                columns: new[] { "PROJ_ID", "PROJ_ATIVO", "PROJ_DATA_ALTERACAO", "PROJ_DATA_EXCLUSAO", "PROJ_DATA_INCLUSAO", "SIS_ID", "PROJ_NOME" },
                values: new object[] { new Guid("ff203e15-8051-4111-8fb4-34aa8eaf72b2"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7031), new Guid("acc21f00-de8d-49e8-9df1-ea74a0148df2"), "Projeto de teste" });

            migrationBuilder.InsertData(
                table: "CPN_TB_TAREFA",
                columns: new[] { "TAR_ID", "TAR_ATIVO", "TAR_DATA_ALTERACAO", "TAR_DATA_EXCLUSAO", "TAR_DATA_INCLUSAO", "TAR_DATA_INICIO", "TAR_DATA_TERMINO", "TAR_DETALHE", "PROJ_ID", "REC_ID", "TIP_ID", "WOR_ID", "TAR_NOME", "TAR_QTD_HORAS" },
                values: new object[] { new Guid("c9c037ab-0dc5-4c0d-8b0e-9ae0a7b718fb"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7080), new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7066), new DateTime(2022, 6, 16, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7066), null, new Guid("ff203e15-8051-4111-8fb4-34aa8eaf72b2"), new Guid("78ad39f8-fc09-454f-a777-350a42a36358"), new Guid("bfbfa056-933c-4419-b2d5-101a3e2fdc33"), new Guid("ac5e6309-16a7-4e37-9bd0-541d9f3cea0b"), "Sistema de teste", 40 });

            migrationBuilder.InsertData(
                table: "CPN_TB_RECURSO_TAREFA",
                columns: new[] { "RTAR_ID", "RTAR_ATIVO", "RTAR_DATA_ALTERACAO", "RTAR_DATA_EXCLUSAO", "RTAR_DATA_INCLUSAO", "REC_ID", "TAR_ID", "RTAR_PERCENTUAL" },
                values: new object[] { new Guid("8681c7e9-fd44-47f2-8d93-79c35fef9137"), true, null, null, new DateTime(2022, 6, 11, 17, 20, 0, 900, DateTimeKind.Local).AddTicks(7084), new Guid("78ad39f8-fc09-454f-a777-350a42a36358"), new Guid("c9c037ab-0dc5-4c0d-8b0e-9ae0a7b718fb"), 25 });

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_APONTAMENTO_TAR_ID",
                table: "CPN_TB_APONTAMENTO",
                column: "TAR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_PROJETO_SIS_ID",
                table: "CPN_TB_PROJETO",
                column: "SIS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_RECURSO_PROJETO_PROJ_ID",
                table: "CPN_TB_RECURSO_PROJETO",
                column: "PROJ_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_RECURSO_PROJETO_REC_ID",
                table: "CPN_TB_RECURSO_PROJETO",
                column: "REC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_RECURSO_TAREFA_REC_ID",
                table: "CPN_TB_RECURSO_TAREFA",
                column: "REC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_RECURSO_TAREFA_TAR_ID",
                table: "CPN_TB_RECURSO_TAREFA",
                column: "TAR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_PROJ_ID",
                table: "CPN_TB_TAREFA",
                column: "PROJ_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_REC_ID",
                table: "CPN_TB_TAREFA",
                column: "REC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_TIP_ID",
                table: "CPN_TB_TAREFA",
                column: "TIP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_WOR_ID",
                table: "CPN_TB_TAREFA",
                column: "WOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_IMPEDIMENTO_IMP_ID",
                table: "CPN_TB_TAREFA_IMPEDIMENTO",
                column: "IMP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CPN_TB_TAREFA_IMPEDIMENTO_TAR_ID",
                table: "CPN_TB_TAREFA_IMPEDIMENTO",
                column: "TAR_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPN_TB_APONTAMENTO");

            migrationBuilder.DropTable(
                name: "CPN_TB_RECURSO_PROJETO");

            migrationBuilder.DropTable(
                name: "CPN_TB_RECURSO_TAREFA");

            migrationBuilder.DropTable(
                name: "CPN_TB_TAREFA_IMPEDIMENTO");

            migrationBuilder.DropTable(
                name: "CPN_TB_IMPEDIMENTO");

            migrationBuilder.DropTable(
                name: "CPN_TB_TAREFA");

            migrationBuilder.DropTable(
                name: "CPN_TB_PROJETO");

            migrationBuilder.DropTable(
                name: "CPN_TB_RECURSO");

            migrationBuilder.DropTable(
                name: "CPN_TB_TIPO_TAREFA");

            migrationBuilder.DropTable(
                name: "CPN_TB_WORKFLOW");

            migrationBuilder.DropTable(
                name: "CPN_TB_SISTEMA");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBanco.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Numero = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Uf = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Tipo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Ativo = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    TipoProdutoBanco = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    ValorMinimo = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    ValorMaximo = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    TaxaJurosMensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    PrazoMaximoMeses = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MdrDebito = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    MdrCreditoAVista = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    MdrCreditoParcelado = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    FaturamentoMinimoMensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    ExigeConvenioEmpregador = table.Column<int>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AgenciaId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TipoCliente = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    RendaMensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    Score = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Cnpj = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    RazaoSocial = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FaturamentoMensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    PessoaJuridica_Score = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Agencias_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "Agencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contratacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ClienteId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ProdutoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MotivoReprovacao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ValorSolicitado = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    PrazoMeses = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "Nome", "PrazoMaximoMeses", "TaxaJurosMensal", "Tipo", "TipoProdutoBanco", "ValorMaximo", "ValorMinimo" },
                values: new object[] { 1L, 1, "Empréstimo Pessoal", 48, 0.035m, 1, "EMPRESTIMO", 50000m, 500m });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "FaturamentoMinimoMensal", "MdrCreditoAVista", "MdrCreditoParcelado", "MdrDebito", "Nome", "Tipo", "TipoProdutoBanco" },
                values: new object[] { 2L, 1, 3000m, 0.0349m, 0.0499m, 0.0199m, "Máquina de Cartão", 2, "MAQUINA_CARTAO" });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "ExigeConvenioEmpregador", "Nome", "Tipo", "TipoProdutoBanco" },
                values: new object[] { 3L, 1, 1, "Receber Salário", 3, "RECEBER_SALARIO" });

            migrationBuilder.CreateIndex(
                name: "IX_Agencias_Numero",
                table: "Agencias",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_AgenciaId",
                table: "Clientes",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cnpj",
                table: "Clientes",
                column: "Cnpj",
                unique: true,
                filter: "\"Cnpj\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cpf",
                table: "Clientes",
                column: "Cpf",
                unique: true,
                filter: "\"Cpf\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_ClienteId",
                table: "Contratacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_ProdutoId",
                table: "Contratacoes",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratacoes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Agencias");
        }
    }
}

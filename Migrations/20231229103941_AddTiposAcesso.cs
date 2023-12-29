using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICadastro.Migrations
{
    /// <inheritdoc />
    public partial class AddTiposAcesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoAcessoId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Inativacoes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "TiposAcesso",
                columns: table => new
                {
                    TipoAcessoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipos = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAcesso", x => x.TipoAcessoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TipoAcessoId",
                table: "Usuarios",
                column: "TipoAcessoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_TiposAcesso_TipoAcessoId",
                table: "Usuarios",
                column: "TipoAcessoId",
                principalTable: "TiposAcesso",
                principalColumn: "TipoAcessoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_TiposAcesso_TipoAcessoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "TiposAcesso");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TipoAcessoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TipoAcessoId",
                table: "Usuarios");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Inativacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Repo.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Clientes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Clientes",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroDoCartao",
                table: "Cartoes",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clientes",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clientes",
                newName: "Nome");

            migrationBuilder.AlterColumn<int>(
                name: "NumeroDoCartao",
                table: "Cartoes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

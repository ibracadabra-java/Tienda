using Microsoft.EntityFrameworkCore.Migrations;

namespace TiendaProducto.Migrations
{
    public partial class UpdateOrden1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "mProducto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "mProducto");
        }
    }
}

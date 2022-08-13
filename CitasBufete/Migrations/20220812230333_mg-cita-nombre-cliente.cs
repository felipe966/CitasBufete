using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitasBufete.Migrations
{
    public partial class mgcitanombrecliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre_cliente",
                table: "Cita",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre_cliente",
                table: "Cita");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitasBufete.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha_hora",
                table: "Cita",
                newName: "Hora");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Cita",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Cita");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "Cita",
                newName: "Fecha_hora");
        }
    }
}

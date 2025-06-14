using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MapeoDeTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Elimino las columnas de tipo time
            migrationBuilder.DropColumn(name: "Holgura",  table: "Tareas");
            migrationBuilder.DropColumn(name: "Duracion", table: "Tareas");

            // 2) Las vuelvo a crear como bigint
            migrationBuilder.AddColumn<long>(
                name: "Holgura",
                table: "Tareas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duracion",
                table: "Tareas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Holgura",  table: "Tareas");
            migrationBuilder.DropColumn(name: "Duracion", table: "Tareas");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Holgura",
                table: "Tareas",
                type: "time",
                nullable: false,
                defaultValue: TimeSpan.Zero);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duracion",
                table: "Tareas",
                type: "time",
                nullable: false,
                defaultValue: TimeSpan.Zero);
        }
    }
}

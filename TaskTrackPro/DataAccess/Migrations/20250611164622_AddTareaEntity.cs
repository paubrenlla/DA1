using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTareaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TareaId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinEstimado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionesProyecto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesProyecto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionesProyecto_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionesProyecto_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EarlyStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarlyFinish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LateFinish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duracion = table.Column<TimeSpan>(type: "time", nullable: false),
                    EsCritica = table.Column<bool>(type: "bit", nullable: false),
                    EstadoValor = table.Column<int>(type: "int", nullable: false),
                    EstadoFecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Holgura = table.Column<TimeSpan>(type: "time", nullable: false),
                    ProyectoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tareas_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareaTarea",
                columns: table => new
                {
                    TareasDependenciaId = table.Column<int>(type: "int", nullable: false),
                    TareasSucesorasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaTarea", x => new { x.TareasDependenciaId, x.TareasSucesorasId });
                    table.ForeignKey(
                        name: "FK_TareaTarea_Tareas_TareasDependenciaId",
                        column: x => x.TareasDependenciaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareaTarea_Tareas_TareasSucesorasId",
                        column: x => x.TareasSucesorasId,
                        principalTable: "Tareas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TareaId",
                table: "Usuarios",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesProyecto_ProyectoId",
                table: "AsignacionesProyecto",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesProyecto_UsuarioId",
                table: "AsignacionesProyecto",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProyectoId",
                table: "Tareas",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaTarea_TareasSucesorasId",
                table: "TareaTarea",
                column: "TareasSucesorasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Tareas_TareaId",
                table: "Usuarios",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Tareas_TareaId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "AsignacionesProyecto");

            migrationBuilder.DropTable(
                name: "TareaTarea");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TareaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TareaId",
                table: "Usuarios");
        }
    }
}

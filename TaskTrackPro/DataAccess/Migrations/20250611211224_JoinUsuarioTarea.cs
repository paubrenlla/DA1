using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class JoinUsuarioTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Tareas_TareaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TareaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TareaId",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "TareaUsuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaUsuario", x => new { x.UsuarioId, x.TareaId });
                    table.ForeignKey(
                        name: "FK_TareaUsuario_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareaUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TareaUsuario_TareaId",
                table: "TareaUsuario",
                column: "TareaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TareaUsuario");

            migrationBuilder.AddColumn<int>(
                name: "TareaId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TareaId",
                table: "Usuarios",
                column: "TareaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Tareas_TareaId",
                table: "Usuarios",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id");
        }
    }
}

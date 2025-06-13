using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TareaUsuario_Tareas_TareaId",
                table: "TareaUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaUsuario_Usuarios_UsuarioId",
                table: "TareaUsuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TareaUsuario",
                table: "TareaUsuario");

            migrationBuilder.RenameTable(
                name: "TareaUsuario",
                newName: "UsuarioTarea");

            migrationBuilder.RenameColumn(
                name: "TareaId",
                table: "UsuarioTarea",
                newName: "UsuariosAsignadosId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "UsuarioTarea",
                newName: "TareasAsignadasId");

            migrationBuilder.RenameIndex(
                name: "IX_TareaUsuario_TareaId",
                table: "UsuarioTarea",
                newName: "IX_UsuarioTarea_UsuariosAsignadosId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioTarea",
                table: "UsuarioTarea",
                columns: new[] { "TareasAsignadasId", "UsuariosAsignadosId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioTarea_Tareas_TareasAsignadasId",
                table: "UsuarioTarea",
                column: "TareasAsignadasId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioTarea_Usuarios_UsuariosAsignadosId",
                table: "UsuarioTarea",
                column: "UsuariosAsignadosId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioTarea_Tareas_TareasAsignadasId",
                table: "UsuarioTarea");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioTarea_Usuarios_UsuariosAsignadosId",
                table: "UsuarioTarea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioTarea",
                table: "UsuarioTarea");

            migrationBuilder.RenameTable(
                name: "UsuarioTarea",
                newName: "TareaUsuario");

            migrationBuilder.RenameColumn(
                name: "UsuariosAsignadosId",
                table: "TareaUsuario",
                newName: "TareaId");

            migrationBuilder.RenameColumn(
                name: "TareasAsignadasId",
                table: "TareaUsuario",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioTarea_UsuariosAsignadosId",
                table: "TareaUsuario",
                newName: "IX_TareaUsuario_TareaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TareaUsuario",
                table: "TareaUsuario",
                columns: new[] { "UsuarioId", "TareaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TareaUsuario_Tareas_TareaId",
                table: "TareaUsuario",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TareaUsuario_Usuarios_UsuarioId",
                table: "TareaUsuario",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

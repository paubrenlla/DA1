using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NotificacionesRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificacionUsuarios_Notificacion_NotificacionesRecibidasId",
                table: "NotificacionUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificacionVistas_Notificacion_NotificacionesVistasId",
                table: "NotificacionVistas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notificacion",
                table: "Notificacion");

            migrationBuilder.RenameTable(
                name: "Notificacion",
                newName: "Notificaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notificaciones",
                table: "Notificaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificacionUsuarios_Notificaciones_NotificacionesRecibidasId",
                table: "NotificacionUsuarios",
                column: "NotificacionesRecibidasId",
                principalTable: "Notificaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificacionVistas_Notificaciones_NotificacionesVistasId",
                table: "NotificacionVistas",
                column: "NotificacionesVistasId",
                principalTable: "Notificaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificacionUsuarios_Notificaciones_NotificacionesRecibidasId",
                table: "NotificacionUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificacionVistas_Notificaciones_NotificacionesVistasId",
                table: "NotificacionVistas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notificaciones",
                table: "Notificaciones");

            migrationBuilder.RenameTable(
                name: "Notificaciones",
                newName: "Notificacion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notificacion",
                table: "Notificacion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificacionUsuarios_Notificacion_NotificacionesRecibidasId",
                table: "NotificacionUsuarios",
                column: "NotificacionesRecibidasId",
                principalTable: "Notificacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificacionVistas_Notificacion_NotificacionesVistasId",
                table: "NotificacionVistas",
                column: "NotificacionesVistasId",
                principalTable: "Notificacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Notificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionUsuarios",
                columns: table => new
                {
                    NotificacionesRecibidasId = table.Column<int>(type: "int", nullable: false),
                    UsuariosNotificadosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionUsuarios", x => new { x.NotificacionesRecibidasId, x.UsuariosNotificadosId });
                    table.ForeignKey(
                        name: "FK_NotificacionUsuarios_Notificacion_NotificacionesRecibidasId",
                        column: x => x.NotificacionesRecibidasId,
                        principalTable: "Notificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificacionUsuarios_Usuarios_UsuariosNotificadosId",
                        column: x => x.UsuariosNotificadosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionVistas",
                columns: table => new
                {
                    NotificacionesVistasId = table.Column<int>(type: "int", nullable: false),
                    VistaPorUsuariosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionVistas", x => new { x.NotificacionesVistasId, x.VistaPorUsuariosId });
                    table.ForeignKey(
                        name: "FK_NotificacionVistas_Notificacion_NotificacionesVistasId",
                        column: x => x.NotificacionesVistasId,
                        principalTable: "Notificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificacionVistas_Usuarios_VistaPorUsuariosId",
                        column: x => x.VistaPorUsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionUsuarios_UsuariosNotificadosId",
                table: "NotificacionUsuarios",
                column: "UsuariosNotificadosId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionVistas_VistaPorUsuariosId",
                table: "NotificacionVistas",
                column: "VistaPorUsuariosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificacionUsuarios");

            migrationBuilder.DropTable(
                name: "NotificacionVistas");

            migrationBuilder.DropTable(
                name: "Notificacion");
        }
    }
}

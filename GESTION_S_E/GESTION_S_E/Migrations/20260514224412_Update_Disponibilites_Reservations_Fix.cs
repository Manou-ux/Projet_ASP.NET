using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class Update_Disponibilites_Reservations_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_clubs_id_club",
                table: "reservations_salle");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_salles_id_salle",
                table: "reservations_salle");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                table: "reservations_salle");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_clubs_id_club",
                table: "reservations_salle",
                column: "id_club",
                principalTable: "clubs",
                principalColumn: "id_club",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_salles_id_salle",
                table: "reservations_salle",
                column: "id_salle",
                principalTable: "salles",
                principalColumn: "id_salle",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                table: "reservations_salle",
                column: "id_utilisateur",
                principalTable: "utilisateurs",
                principalColumn: "id_utilisateur",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_clubs_id_club",
                table: "reservations_salle");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_salles_id_salle",
                table: "reservations_salle");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                table: "reservations_salle");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_clubs_id_club",
                table: "reservations_salle",
                column: "id_club",
                principalTable: "clubs",
                principalColumn: "id_club");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_salles_id_salle",
                table: "reservations_salle",
                column: "id_salle",
                principalTable: "salles",
                principalColumn: "id_salle",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                table: "reservations_salle",
                column: "id_utilisateur",
                principalTable: "utilisateurs",
                principalColumn: "id_utilisateur",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

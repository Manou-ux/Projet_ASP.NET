using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableReservationSalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservations_salle",
                columns: table => new
                {
                    id_reservation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_salle = table.Column<int>(type: "integer", nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false),
                    id_club = table.Column<int>(type: "integer", nullable: true),
                    date_reservation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    heure_debut = table.Column<TimeSpan>(type: "interval", nullable: false),
                    heure_fin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    motif = table.Column<string>(type: "text", nullable: false),
                    statut = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations_salle", x => x.id_reservation);
                    table.CheckConstraint("CK_Reservation_Statut", "statut IN ('en_attente','validee','annulee')");
                    table.ForeignKey(
                        name: "FK_reservations_salle_clubs_id_club",
                        column: x => x.id_club,
                        principalTable: "clubs",
                        principalColumn: "id_club");
                    table.ForeignKey(
                        name: "FK_reservations_salle_salles_id_salle",
                        column: x => x.id_salle,
                        principalTable: "salles",
                        principalColumn: "id_salle",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_club",
                table: "reservations_salle",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_salle",
                table: "reservations_salle",
                column: "id_salle");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_utilisateur",
                table: "reservations_salle",
                column: "id_utilisateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations_salle");
        }
    }
}

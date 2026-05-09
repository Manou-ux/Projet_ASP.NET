using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableMembreClub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "membres_club",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false),
                    id_club = table.Column<int>(type: "integer", nullable: false),
                    role_membre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date_adhesion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membres_club", x => new { x.id_utilisateur, x.id_club });
                    table.CheckConstraint("CK_Membre_Role", "role_membre IN ('president','tresorier','secretaire','membre')");
                    table.ForeignKey(
                        name: "FK_membres_club_clubs_id_club",
                        column: x => x.id_club,
                        principalTable: "clubs",
                        principalColumn: "id_club",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_membres_club_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_membres_club_id_club",
                table: "membres_club",
                column: "id_club");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "membres_club");
        }
    }
}

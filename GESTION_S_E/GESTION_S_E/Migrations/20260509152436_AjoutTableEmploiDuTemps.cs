using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableEmploiDuTemps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "emplois_du_temps",
                columns: table => new
                {
                    id_emploi = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_cours = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    heure_debut = table.Column<TimeSpan>(type: "interval", nullable: false),
                    heure_fin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    semestre = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    statut = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    id_salle = table.Column<int>(type: "integer", nullable: false),
                    id_enseignant = table.Column<int>(type: "integer", nullable: false),
                    id_matiere = table.Column<int>(type: "integer", nullable: false),
                    id_classe = table.Column<int>(type: "integer", nullable: true),
                    id_groupe = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emplois_du_temps", x => x.id_emploi);
                    table.CheckConstraint("CK_Emploi_Destinataire", "id_classe IS NOT NULL OR id_groupe IS NOT NULL");
                    table.CheckConstraint("CK_Emploi_Semestre", "semestre IN ('S1','S2','S3','S4','S5','S6')");
                    table.CheckConstraint("CK_Emploi_Statut", "statut IN ('planifie','en_cours','termine','annule','reporte')");
                    table.ForeignKey(
                        name: "FK_emplois_du_temps_classes_id_classe",
                        column: x => x.id_classe,
                        principalTable: "classes",
                        principalColumn: "id_classe");
                    table.ForeignKey(
                        name: "FK_emplois_du_temps_enseignants_id_enseignant",
                        column: x => x.id_enseignant,
                        principalTable: "enseignants",
                        principalColumn: "id_enseignant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_emplois_du_temps_groupes_id_groupe",
                        column: x => x.id_groupe,
                        principalTable: "groupes",
                        principalColumn: "id_groupe");
                    table.ForeignKey(
                        name: "FK_emplois_du_temps_matieres_id_matiere",
                        column: x => x.id_matiere,
                        principalTable: "matieres",
                        principalColumn: "id_matiere",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_emplois_du_temps_salles_id_salle",
                        column: x => x.id_salle,
                        principalTable: "salles",
                        principalColumn: "id_salle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_emplois_du_temps_id_classe",
                table: "emplois_du_temps",
                column: "id_classe");

            migrationBuilder.CreateIndex(
                name: "IX_emplois_du_temps_id_enseignant",
                table: "emplois_du_temps",
                column: "id_enseignant");

            migrationBuilder.CreateIndex(
                name: "IX_emplois_du_temps_id_groupe",
                table: "emplois_du_temps",
                column: "id_groupe");

            migrationBuilder.CreateIndex(
                name: "IX_emplois_du_temps_id_matiere",
                table: "emplois_du_temps",
                column: "id_matiere");

            migrationBuilder.CreateIndex(
                name: "IX_emplois_du_temps_id_salle",
                table: "emplois_du_temps",
                column: "id_salle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emplois_du_temps");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableDisponibiliteEnseignant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "disponibilites_enseignants",
                columns: table => new
                {
                    id_dispo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_enseignant = table.Column<int>(type: "integer", nullable: false),
                    jour = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    heure_debut = table.Column<TimeSpan>(type: "interval", nullable: false),
                    heure_fin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    type_dispo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    date_specifique = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disponibilites_enseignants", x => x.id_dispo);
                    table.CheckConstraint("CK_Dispo_Jour", "jour IN ('Lundi','Mardi','Mercredi','Jeudi','Vendredi','Samedi')");
                    table.CheckConstraint("CK_Dispo_Type", "type_dispo IN ('cours','td','tp','reunion')");
                    table.ForeignKey(
                        name: "FK_disponibilites_enseignants_enseignants_id_enseignant",
                        column: x => x.id_enseignant,
                        principalTable: "enseignants",
                        principalColumn: "id_enseignant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_disponibilites_enseignants_id_enseignant",
                table: "disponibilites_enseignants",
                column: "id_enseignant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "disponibilites_enseignants");
        }
    }
}

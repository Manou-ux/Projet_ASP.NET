using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableEleve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eleves",
                columns: table => new
                {
                    id_eleve = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_eleve = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom_eleve = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date_naissance = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    matricule = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    id_classe = table.Column<int>(type: "integer", nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eleves", x => x.id_eleve);
                    table.ForeignKey(
                        name: "FK_eleves_classes_id_classe",
                        column: x => x.id_classe,
                        principalTable: "classes",
                        principalColumn: "id_classe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_eleves_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eleves_id_classe",
                table: "eleves",
                column: "id_classe");

            migrationBuilder.CreateIndex(
                name: "IX_eleves_id_utilisateur",
                table: "eleves",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_eleves_matricule",
                table: "eleves",
                column: "matricule",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eleves");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableEnseignant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enseignants",
                columns: table => new
                {
                    id_enseignant = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_enseignant = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom_enseignant = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    specialite = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telephone_enseignant = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email_pro = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enseignants", x => x.id_enseignant);
                    table.ForeignKey(
                        name: "FK_enseignants_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_enseignants_id_utilisateur",
                table: "enseignants",
                column: "id_utilisateur",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enseignants");
        }
    }
}

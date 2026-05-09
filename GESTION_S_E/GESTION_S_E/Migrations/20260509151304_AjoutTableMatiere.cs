using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableMatiere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matieres",
                columns: table => new
                {
                    id_matiere = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_matiere = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    code_matiere = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    volume_horaire = table.Column<int>(type: "integer", nullable: true),
                    coefficient = table.Column<decimal>(type: "numeric(3,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matieres", x => x.id_matiere);
                });

            migrationBuilder.CreateIndex(
                name: "IX_matieres_code_matiere",
                table: "matieres",
                column: "code_matiere",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matieres");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableSalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salles",
                columns: table => new
                {
                    id_salle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_salle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    capacite = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    localisation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salles", x => x.id_salle);
                    table.CheckConstraint("CK_Salle_Type", "\"type\" IN ('TP','cours','amphi','reunion')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_salles_nom_salle",
                table: "salles",
                column: "nom_salle",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salles");
        }
    }
}

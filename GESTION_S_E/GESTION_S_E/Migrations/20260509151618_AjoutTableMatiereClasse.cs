using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableMatiereClasse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matiere_classe",
                columns: table => new
                {
                    id_matiere = table.Column<int>(type: "integer", nullable: false),
                    id_classe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matiere_classe", x => new { x.id_matiere, x.id_classe });
                    table.ForeignKey(
                        name: "FK_matiere_classe_classes_id_classe",
                        column: x => x.id_classe,
                        principalTable: "classes",
                        principalColumn: "id_classe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_matiere_classe_matieres_id_matiere",
                        column: x => x.id_matiere,
                        principalTable: "matieres",
                        principalColumn: "id_matiere",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_matiere_classe_id_classe",
                table: "matiere_classe",
                column: "id_classe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matiere_classe");
        }
    }
}

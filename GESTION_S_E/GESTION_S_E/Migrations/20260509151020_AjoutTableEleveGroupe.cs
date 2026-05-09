using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableEleveGroupe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eleve_groupe",
                columns: table => new
                {
                    id_eleve = table.Column<int>(type: "integer", nullable: false),
                    id_groupe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eleve_groupe", x => new { x.id_eleve, x.id_groupe });
                    table.ForeignKey(
                        name: "FK_eleve_groupe_eleves_id_eleve",
                        column: x => x.id_eleve,
                        principalTable: "eleves",
                        principalColumn: "id_eleve",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_eleve_groupe_groupes_id_groupe",
                        column: x => x.id_groupe,
                        principalTable: "groupes",
                        principalColumn: "id_groupe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eleve_groupe_id_groupe",
                table: "eleve_groupe",
                column: "id_groupe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eleve_groupe");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableClasseCorrige : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id_classe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_classe = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Niveau = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Filiere = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Effectif = table.Column<int>(type: "integer", nullable: false),
                    annee_academique = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id_classe);
                    table.CheckConstraint("CK_Classe_Niveau", "\"Niveau\" IN ('L1','L2','L3','M1','M2')");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classes");
        }
    }
}

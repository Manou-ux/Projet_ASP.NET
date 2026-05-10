using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GESTION_S_E.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    nom_classe = table.Column<string>(type: "text", nullable: false),
                    Niveau = table.Column<string>(type: "text", nullable: false),
                    Filiere = table.Column<string>(type: "text", nullable: false),
                    Effectif = table.Column<int>(type: "integer", nullable: false),
                    annee_academique = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id_classe);
                    table.CheckConstraint("CK_Classe_Niveau", "\"Niveau\" IN ('L1','L2','L3','M1','M2')");
                });

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

            migrationBuilder.CreateTable(
                name: "salles",
                columns: table => new
                {
                    id_salle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_salle = table.Column<string>(type: "text", nullable: false),
                    capacite = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    localisation = table.Column<string>(type: "text", nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salles", x => x.id_salle);
                    table.CheckConstraint("CK_Salle_Type", "\"type\" IN ('TP','cours','amphi','reunion')");
                });

            migrationBuilder.CreateTable(
                name: "utilisateurs",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MotDePasse = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Actif = table.Column<bool>(type: "boolean", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilisateurs", x => x.id_utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "groupes",
                columns: table => new
                {
                    id_groupe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_groupe = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_classe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupes", x => x.id_groupe);
                    table.ForeignKey(
                        name: "FK_groupes_classes_id_classe",
                        column: x => x.id_classe,
                        principalTable: "classes",
                        principalColumn: "id_classe",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "clubs",
                columns: table => new
                {
                    id_club = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_club = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    id_responsable = table.Column<int>(type: "integer", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubs", x => x.id_club);
                    table.ForeignKey(
                        name: "FK_clubs_utilisateurs_id_responsable",
                        column: x => x.id_responsable,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id_notification = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    date_envoi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lu = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id_notification);
                    table.ForeignKey(
                        name: "FK_notifications_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scolarites",
                columns: table => new
                {
                    id_scolarite = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_scolarite = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom_scolarite = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fonction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    bureau = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scolarites", x => x.id_scolarite);
                    table.ForeignKey(
                        name: "FK_scolarites_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "reservations_salle",
                columns: table => new
                {
                    id_reservation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_salle = table.Column<int>(type: "integer", nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false),
                    id_club = table.Column<int>(type: "integer", nullable: true),
                    date_reservation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    heure_debut = table.Column<TimeSpan>(type: "interval", nullable: false),
                    heure_fin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    motif = table.Column<string>(type: "text", nullable: false),
                    statut = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations_salle", x => x.id_reservation);
                    table.CheckConstraint("CK_Reservation_Statut", "statut IN ('en_attente','validee','annulee')");
                    table.ForeignKey(
                        name: "FK_reservations_salle_clubs_id_club",
                        column: x => x.id_club,
                        principalTable: "clubs",
                        principalColumn: "id_club");
                    table.ForeignKey(
                        name: "FK_reservations_salle_salles_id_salle",
                        column: x => x.id_salle,
                        principalTable: "salles",
                        principalColumn: "id_salle",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_salle_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_clubs_id_responsable",
                table: "clubs",
                column: "id_responsable");

            migrationBuilder.CreateIndex(
                name: "IX_clubs_nom_club",
                table: "clubs",
                column: "nom_club",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_disponibilites_enseignants_id_enseignant",
                table: "disponibilites_enseignants",
                column: "id_enseignant");

            migrationBuilder.CreateIndex(
                name: "IX_eleve_groupe_id_groupe",
                table: "eleve_groupe",
                column: "id_groupe");

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

            migrationBuilder.CreateIndex(
                name: "IX_enseignants_id_utilisateur",
                table: "enseignants",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groupes_id_classe",
                table: "groupes",
                column: "id_classe");

            migrationBuilder.CreateIndex(
                name: "IX_matiere_classe_id_classe",
                table: "matiere_classe",
                column: "id_classe");

            migrationBuilder.CreateIndex(
                name: "IX_matieres_code_matiere",
                table: "matieres",
                column: "code_matiere",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_membres_club_id_club",
                table: "membres_club",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_id_utilisateur",
                table: "notifications",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_club",
                table: "reservations_salle",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_salle",
                table: "reservations_salle",
                column: "id_salle");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_salle_id_utilisateur",
                table: "reservations_salle",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_salles_nom_salle",
                table: "salles",
                column: "nom_salle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scolarites_id_utilisateur",
                table: "scolarites",
                column: "id_utilisateur",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "disponibilites_enseignants");

            migrationBuilder.DropTable(
                name: "eleve_groupe");

            migrationBuilder.DropTable(
                name: "emplois_du_temps");

            migrationBuilder.DropTable(
                name: "matiere_classe");

            migrationBuilder.DropTable(
                name: "membres_club");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "reservations_salle");

            migrationBuilder.DropTable(
                name: "scolarites");

            migrationBuilder.DropTable(
                name: "eleves");

            migrationBuilder.DropTable(
                name: "enseignants");

            migrationBuilder.DropTable(
                name: "groupes");

            migrationBuilder.DropTable(
                name: "matieres");

            migrationBuilder.DropTable(
                name: "clubs");

            migrationBuilder.DropTable(
                name: "salles");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "utilisateurs");
        }
    }
}

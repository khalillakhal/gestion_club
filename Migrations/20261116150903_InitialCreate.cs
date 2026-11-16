using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personnes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    Prenom = table.Column<string>(type: "TEXT", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    TypePersonne = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Specialite = table.Column<string>(type: "TEXT", nullable: true),
                    DateInscription = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CotisationPayee = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Lieu = table.Column<string>(type: "TEXT", nullable: true),
                    Categorie = table.Column<string>(type: "TEXT", nullable: true),
                    DisciplineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duree = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Lieu = table.Column<string>(type: "TEXT", nullable: true),
                    Annulee = table.Column<bool>(type: "INTEGER", nullable: false),
                    EntraineurId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisciplineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seances_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seances_Personnes_EntraineurId",
                        column: x => x.EntraineurId,
                        principalTable: "Personnes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    MembreId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Resultat = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => new { x.MembreId, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_Inscriptions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Personnes_MembreId",
                        column: x => x.MembreId,
                        principalTable: "Personnes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeanceEntrainementId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => new { x.ParticipantsId, x.SeanceEntrainementId });
                    table.ForeignKey(
                        name: "FK_Participations_Personnes_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Personnes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participations_Seances_SeanceEntrainementId",
                        column: x => x.SeanceEntrainementId,
                        principalTable: "Seances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_DisciplineId",
                table: "Competitions",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CompetitionId",
                table: "Inscriptions",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_SeanceEntrainementId",
                table: "Participations",
                column: "SeanceEntrainementId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_DisciplineId",
                table: "Seances",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_EntraineurId",
                table: "Seances",
                column: "EntraineurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropTable(
                name: "Personnes");
        }
    }
}

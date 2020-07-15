using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class tvshows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeActor");

            migrationBuilder.DropTable(
                name: "EpisodeDirector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TVShowReview",
                table: "TVShowReview");

            migrationBuilder.DropIndex(
                name: "IX_TVShowReview_TVShowId",
                table: "TVShowReview");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "TVShows",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "TVShows",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EpisodeNumber",
                table: "Episodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TVShowReview",
                table: "TVShowReview",
                columns: new[] { "TVShowId", "ReviewId" });

            migrationBuilder.CreateIndex(
                name: "IX_TVShowReview_EpisodeId",
                table: "TVShowReview",
                column: "EpisodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TVShowReview",
                table: "TVShowReview");

            migrationBuilder.DropIndex(
                name: "IX_TVShowReview_EpisodeId",
                table: "TVShowReview");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "EpisodeNumber",
                table: "Episodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TVShowReview",
                table: "TVShowReview",
                columns: new[] { "EpisodeId", "ReviewId" });

            migrationBuilder.CreateTable(
                name: "EpisodeActor",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeActor", x => new { x.EpisodeId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_EpisodeActor_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeActor_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeDirector",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(type: "int", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeDirector", x => new { x.EpisodeId, x.DirectorId });
                    table.ForeignKey(
                        name: "FK_EpisodeDirector_Directors_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Directors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeDirector_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TVShowReview_TVShowId",
                table: "TVShowReview",
                column: "TVShowId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeActor_ActorId",
                table: "EpisodeActor",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeDirector_DirectorId",
                table: "EpisodeDirector",
                column: "DirectorId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class ReviewRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reviews");

            migrationBuilder.CreateTable(
                name: "MovieReview",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieReview", x => new { x.MovieId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_MovieReview_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieReview_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicReview",
                columns: table => new
                {
                    AlbumId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: false),
                    SongId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicReview", x => new { x.AlbumId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_MusicReview_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicReview_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicReview_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodcastReview",
                columns: table => new
                {
                    PodcastId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastReview", x => new { x.PodcastId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_PodcastReview_PodCasts_PodcastId",
                        column: x => x.PodcastId,
                        principalTable: "PodCasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PodcastReview_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowReview",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: false),
                    TVShowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowReview", x => new { x.EpisodeId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_TVShowReview_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowReview_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowReview_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieReview_ReviewId",
                table: "MovieReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicReview_ReviewId",
                table: "MusicReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicReview_SongId",
                table: "MusicReview",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastReview_ReviewId",
                table: "PodcastReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowReview_ReviewId",
                table: "TVShowReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowReview_TVShowId",
                table: "TVShowReview",
                column: "TVShowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieReview");

            migrationBuilder.DropTable(
                name: "MusicReview");

            migrationBuilder.DropTable(
                name: "PodcastReview");

            migrationBuilder.DropTable(
                name: "TVShowReview");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

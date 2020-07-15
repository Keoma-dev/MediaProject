using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class mtmplaylistsong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayLists_AspNetUsers_MediaWebUserId1",
                table: "PlayLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_PlayLists_PlayListId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_PlayListId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_PlayLists_MediaWebUserId1",
                table: "PlayLists");

            migrationBuilder.DropColumn(
                name: "PlayListId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "MediaWebUserId1",
                table: "PlayLists");

            migrationBuilder.AlterColumn<string>(
                name: "MediaWebUserId",
                table: "PlayLists",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "PlayLists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PlaylistSong",
                columns: table => new
                {
                    SongId = table.Column<int>(nullable: false),
                    PlayListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSong", x => new { x.SongId, x.PlayListId });
                    table.ForeignKey(
                        name: "FK_PlaylistSong_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSong_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_MediaWebUserId",
                table: "PlayLists",
                column: "MediaWebUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSong_PlayListId",
                table: "PlaylistSong",
                column: "PlayListId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayLists_AspNetUsers_MediaWebUserId",
                table: "PlayLists",
                column: "MediaWebUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayLists_AspNetUsers_MediaWebUserId",
                table: "PlayLists");

            migrationBuilder.DropTable(
                name: "PlaylistSong");

            migrationBuilder.DropIndex(
                name: "IX_PlayLists_MediaWebUserId",
                table: "PlayLists");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "PlayLists");

            migrationBuilder.AddColumn<int>(
                name: "PlayListId",
                table: "Songs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediaWebUserId",
                table: "PlayLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaWebUserId1",
                table: "PlayLists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlayListId",
                table: "Songs",
                column: "PlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_MediaWebUserId1",
                table: "PlayLists",
                column: "MediaWebUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayLists_AspNetUsers_MediaWebUserId1",
                table: "PlayLists",
                column: "MediaWebUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_PlayLists_PlayListId",
                table: "Songs",
                column: "PlayListId",
                principalTable: "PlayLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

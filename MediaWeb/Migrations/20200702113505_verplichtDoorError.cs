using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class verplichtDoorError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_MediaWebUserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MediaWebUserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MediaWebUserId1",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "MediaWebUserId",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaWebUserId",
                table: "Reviews",
                column: "MediaWebUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_MediaWebUserId",
                table: "Reviews",
                column: "MediaWebUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_MediaWebUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MediaWebUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "MediaWebUserId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaWebUserId1",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaWebUserId1",
                table: "Reviews",
                column: "MediaWebUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_MediaWebUserId1",
                table: "Reviews",
                column: "MediaWebUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

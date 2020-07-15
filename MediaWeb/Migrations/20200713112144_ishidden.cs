using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class ishidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "TVShows",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Songs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "PodCasts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "PodCasts");
        }
    }
}

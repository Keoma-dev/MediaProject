using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaWeb.Migrations
{
    public partial class podcastfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "PodCasts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "PodCasts");
        }
    }
}

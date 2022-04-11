using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageShareWithLikes.Data.Migrations
{
    public partial class changedtolikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "View",
                table: "Images",
                newName: "Likes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Images",
                newName: "View");
        }
    }
}

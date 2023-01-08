using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForumProject.DAL.Migrations
{
    public partial class m4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogName",
                schema: "Identity",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogName",
                schema: "Identity",
                table: "Posts");
        }
    }
}

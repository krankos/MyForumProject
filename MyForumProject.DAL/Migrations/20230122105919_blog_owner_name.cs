using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForumProject.DAL.Migrations
{
    public partial class blog_owner_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                schema: "Identity",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                schema: "Identity",
                table: "Blogs");
        }
    }
}

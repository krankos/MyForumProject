using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForumProject.DAL.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "Identity",
                table: "Blogs",
                newName: "description");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                schema: "Identity",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nom",
                schema: "Identity",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "Identity",
                table: "Blogs",
                newName: "Url");
        }
    }
}

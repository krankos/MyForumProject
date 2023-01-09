using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForumProject.DAL.Migrations
{
    public partial class comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogName",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                schema: "Identity",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                schema: "Identity",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "Identity",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Identity",
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                schema: "Identity",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_OwnerId",
                schema: "Identity",
                table: "Blogs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerId",
                schema: "Identity",
                table: "Comments",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_OwnerId",
                schema: "Identity",
                table: "Blogs",
                column: "OwnerId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_OwnerId",
                schema: "Identity",
                table: "Posts",
                column: "OwnerId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_OwnerId",
                schema: "Identity",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_OwnerId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "IX_Posts_OwnerId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_OwnerId",
                schema: "Identity",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Identity",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "BlogName",
                schema: "Identity",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

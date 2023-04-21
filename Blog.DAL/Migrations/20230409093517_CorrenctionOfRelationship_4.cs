using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrenctionOfRelationship_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable",
                column: "ParentId",
                principalTable: "Commentable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable",
                column: "ParentId",
                principalTable: "Commentable",
                principalColumn: "Id");
        }
    }
}

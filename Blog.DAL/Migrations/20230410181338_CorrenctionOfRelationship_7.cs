using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrenctionOfRelationship_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rubrics_Rubrics_RubricId",
                table: "Rubrics");

            migrationBuilder.RenameColumn(
                name: "RubricId",
                table: "Rubrics",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Rubrics_RubricId",
                table: "Rubrics",
                newName: "IX_Rubrics_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rubrics_Rubrics_ParentId",
                table: "Rubrics",
                column: "ParentId",
                principalTable: "Rubrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rubrics_Rubrics_ParentId",
                table: "Rubrics");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Rubrics",
                newName: "RubricId");

            migrationBuilder.RenameIndex(
                name: "IX_Rubrics_ParentId",
                table: "Rubrics",
                newName: "IX_Rubrics_RubricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rubrics_Rubrics_RubricId",
                table: "Rubrics",
                column: "RubricId",
                principalTable: "Rubrics",
                principalColumn: "Id");
        }
    }
}

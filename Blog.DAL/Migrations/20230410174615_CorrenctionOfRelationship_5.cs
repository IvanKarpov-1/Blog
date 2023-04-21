using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrenctionOfRelationship_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RubricId",
                table: "Rubrics",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_RubricId",
                table: "Rubrics",
                column: "RubricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rubrics_Rubrics_RubricId",
                table: "Rubrics",
                column: "RubricId",
                principalTable: "Rubrics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rubrics_Rubrics_RubricId",
                table: "Rubrics");

            migrationBuilder.DropIndex(
                name: "IX_Rubrics_RubricId",
                table: "Rubrics");

            migrationBuilder.DropColumn(
                name: "RubricId",
                table: "Rubrics");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrenctionOfRelationship_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rubrics_Rubrics_ParentId",
                table: "Rubrics");

            migrationBuilder.DropIndex(
                name: "IX_Rubrics_ParentId",
                table: "Rubrics");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Rubrics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Rubrics",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_ParentId",
                table: "Rubrics",
                column: "ParentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rubrics_Rubrics_ParentId",
                table: "Rubrics",
                column: "ParentId",
                principalTable: "Rubrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

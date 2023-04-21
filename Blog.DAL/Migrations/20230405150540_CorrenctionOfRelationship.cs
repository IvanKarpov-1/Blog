using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrenctionOfRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentId",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Commentable");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Commentable",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Commentable",
                newName: "RubricId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorId",
                table: "Commentable",
                newName: "IX_Commentable_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ArticleId",
                table: "Commentable",
                newName: "IX_Commentable_RubricId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Commentable",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Commentable",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Article_AuthorId",
                table: "Commentable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Article_Content",
                table: "Commentable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Article_CreatedDate",
                table: "Commentable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Commentable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Commentable",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Commentable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commentable",
                table: "Commentable",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Commentable_Article_AuthorId",
                table: "Commentable",
                column: "Article_AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentable_ParentId",
                table: "Commentable",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_AspNetUsers_Article_AuthorId",
                table: "Commentable",
                column: "Article_AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_AspNetUsers_AuthorId",
                table: "Commentable",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable",
                column: "ParentId",
                principalTable: "Commentable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentable_Rubrics_RubricId",
                table: "Commentable",
                column: "RubricId",
                principalTable: "Rubrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_AspNetUsers_Article_AuthorId",
                table: "Commentable");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_AspNetUsers_AuthorId",
                table: "Commentable");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_Commentable_ParentId",
                table: "Commentable");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentable_Rubrics_RubricId",
                table: "Commentable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commentable",
                table: "Commentable");

            migrationBuilder.DropIndex(
                name: "IX_Commentable_Article_AuthorId",
                table: "Commentable");

            migrationBuilder.DropIndex(
                name: "IX_Commentable_ParentId",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "Article_AuthorId",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "Article_Content",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "Article_CreatedDate",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Commentable");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Commentable");

            migrationBuilder.RenameTable(
                name: "Commentable",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "RubricId",
                table: "Comments",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Commentable_RubricId",
                table: "Comments",
                newName: "IX_Comments_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Commentable_AuthorId",
                table: "Comments",
                newName: "IX_Comments_AuthorId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: true),
                    RubricId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Rubrics_RubricId",
                        column: x => x.RubricId,
                        principalTable: "Rubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_RubricId",
                table: "Articles",
                column: "RubricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}

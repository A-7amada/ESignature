using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BTIT.EPM.Migrations
{
    public partial class fileupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DocumentBagId",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "AppBinaryObjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppBinaryObjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "AppBinaryObjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "AppBinaryObjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentBags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DocumentBagId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentBags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentBagId",
                table: "Documents",
                column: "DocumentBagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentBags_DocumentBagId",
                table: "Documents",
                column: "DocumentBagId",
                principalTable: "DocumentBags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentBags_DocumentBagId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentBags");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentBagId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentBagId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "AppBinaryObjects");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppBinaryObjects");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "AppBinaryObjects");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "AppBinaryObjects");
        }
    }
}

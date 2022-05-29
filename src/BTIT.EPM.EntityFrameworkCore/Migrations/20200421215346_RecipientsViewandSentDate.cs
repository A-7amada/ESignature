using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BTIT.EPM.Migrations
{
    public partial class RecipientsViewandSentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Recipients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Recipients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Recipients");
        }
    }
}

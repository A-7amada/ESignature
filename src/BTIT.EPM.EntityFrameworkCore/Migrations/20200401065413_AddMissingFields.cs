using Microsoft.EntityFrameworkCore.Migrations;

namespace BTIT.EPM.Migrations
{
    public partial class AddMissingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "Recipients",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SigneOrder",
                table: "Recipients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                table: "DocumentRequestAuditTrails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequestAuditTrails_RecipientId",
                table: "DocumentRequestAuditTrails",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentRequestAuditTrails_Recipients_RecipientId",
                table: "DocumentRequestAuditTrails",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentRequestAuditTrails_Recipients_RecipientId",
                table: "DocumentRequestAuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_DocumentRequestAuditTrails_RecipientId",
                table: "DocumentRequestAuditTrails");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "SigneOrder",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "DocumentRequestAuditTrails");
        }
    }
}

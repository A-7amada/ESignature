using Microsoft.EntityFrameworkCore.Migrations;

namespace BTIT.EPM.Migrations
{
    public partial class Added_FileSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileSignatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Describtion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSignatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileSignatures_TenantId",
                table: "FileSignatures",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileSignatures");
        }
    }
}

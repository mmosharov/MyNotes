using Microsoft.EntityFrameworkCore.Migrations;

namespace MyNotes.Migrations
{
    public partial class AddNotesSharings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotesSharings",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false),
                    ShareWithUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotesSharings", x => x.NoteId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotesSharings_ShareWithUserId",
                table: "NotesSharings",
                column: "ShareWithUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotesSharings");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");
        }
    }
}

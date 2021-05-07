using Microsoft.EntityFrameworkCore.Migrations;

namespace MyNotes.Migrations
{
    public partial class NotesSharingCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotesSharings",
                table: "NotesSharings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotesSharings",
                table: "NotesSharings",
                columns: new[] { "NoteId", "ShareWithUserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotesSharings",
                table: "NotesSharings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotesSharings",
                table: "NotesSharings",
                column: "NoteId");
        }
    }
}

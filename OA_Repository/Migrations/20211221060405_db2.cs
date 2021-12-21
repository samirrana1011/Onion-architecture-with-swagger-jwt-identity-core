using Microsoft.EntityFrameworkCore.Migrations;

namespace OA_Repository.Migrations
{
    public partial class db2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_AspNetUsers_UserID",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_UserID",
                table: "Note");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Note",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Note",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_UserID",
                table: "Note",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_AspNetUsers_UserID",
                table: "Note",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

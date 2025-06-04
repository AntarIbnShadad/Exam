using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam.Migrations
{
    /// <inheritdoc />
    public partial class IndentityMigrationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserName",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Students",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_UserName",
                table: "Students",
                newName: "IX_Students_UserId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Instructors",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Instructors_UserName",
                table: "Instructors",
                newName: "IX_Instructors_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Students",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Students_UserId",
                table: "Students",
                newName: "IX_Students_UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Instructors",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                newName: "IX_Instructors_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserName",
                table: "Instructors",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserName",
                table: "Students",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

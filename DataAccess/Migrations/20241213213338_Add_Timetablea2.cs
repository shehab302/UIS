using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Timetablea2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken",
                schema: "Securty",
                newName: "IdentityUserToken",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole",
                schema: "Securty",
                newName: "IdentityUserRole",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin",
                schema: "Securty",
                newName: "IdentityUserLogin",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim",
                schema: "Securty",
                newName: "IdentityUserClaim",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                schema: "Securty",
                newName: "IdentityUser",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim",
                schema: "Securty",
                newName: "IdentityRoleClaim",
                newSchema: "Security");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                schema: "Securty",
                newName: "IdentityRole",
                newSchema: "Security");

            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    TimetableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    LectureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.TimetableId);
                    table.ForeignKey(
                        name: "FK_Timetables_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "LecturesId");
                    table.ForeignKey(
                        name: "FK_Timetables_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionsId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_LectureId",
                table: "Timetables",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_SectionId",
                table: "Timetables",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetables");

            migrationBuilder.EnsureSchema(
                name: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken",
                schema: "Security",
                newName: "IdentityUserToken",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole",
                schema: "Security",
                newName: "IdentityUserRole",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin",
                schema: "Security",
                newName: "IdentityUserLogin",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim",
                schema: "Security",
                newName: "IdentityUserClaim",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                schema: "Security",
                newName: "IdentityUser",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim",
                schema: "Security",
                newName: "IdentityRoleClaim",
                newSchema: "Securty");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                schema: "Security",
                newName: "IdentityRole",
                newSchema: "Securty");
        }
    }
}

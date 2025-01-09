using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class droptimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Lectures_LectureId",
                table: "Timetables");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Sections_SectionId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_LectureId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_SectionId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "isDay",
                table: "Timetables");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "Timetables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Timetables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDay",
                table: "Timetables",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_LectureId",
                table: "Timetables",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_SectionId",
                table: "Timetables",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_Lectures_LectureId",
                table: "Timetables",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "LecturesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_Sections_SectionId",
                table: "Timetables",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionsId");
        }
    }
}

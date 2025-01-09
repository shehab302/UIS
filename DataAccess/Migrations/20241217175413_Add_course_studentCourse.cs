using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_course_studentCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "degree",
                table: "StudentCourses",
                newName: "Degree");

            migrationBuilder.RenameColumn(
                name: "Hourse",
                table: "Courses",
                newName: "PracticalDegree");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "StudentCourses",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentAttendancedegree",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentFinalDegree",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentMidTermDegree",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentOralDegree",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentPracticalDegree",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceDegree",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinalDegree",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MidTermDegree",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OralDegree",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "StudentAttendancedegree",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "StudentFinalDegree",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "StudentMidTermDegree",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "StudentOralDegree",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "StudentPracticalDegree",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "AttendanceDegree",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FinalDegree",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MidTermDegree",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OralDegree",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "StudentCourses",
                newName: "degree");

            migrationBuilder.RenameColumn(
                name: "PracticalDegree",
                table: "Courses",
                newName: "Hourse");
        }
    }
}

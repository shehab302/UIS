using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editInquiryEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inquiry",
                table: "Inquiry");

            migrationBuilder.RenameTable(
                name: "Inquiry",
                newName: "Inquirys");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Inquirys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inquirys",
                table: "Inquirys",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inquirys",
                table: "Inquirys");

            migrationBuilder.RenameTable(
                name: "Inquirys",
                newName: "Inquiry");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Inquiry",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inquiry",
                table: "Inquiry",
                column: "Id");
        }
    }
}

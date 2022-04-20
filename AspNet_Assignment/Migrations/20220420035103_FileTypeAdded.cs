using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNet_Assignment.Migrations
{
    public partial class FileTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "FileUploaded",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "FileUploaded");
        }
    }
}

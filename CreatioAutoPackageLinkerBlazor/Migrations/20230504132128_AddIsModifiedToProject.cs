using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreatioAutoPackageLinkerBlazor.Migrations
{
    public partial class AddIsModifiedToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsModified",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsModified",
                table: "Projects");
        }
    }
}

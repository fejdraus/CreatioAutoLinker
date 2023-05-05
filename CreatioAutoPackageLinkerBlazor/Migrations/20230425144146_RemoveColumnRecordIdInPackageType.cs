using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreatioAutoPackageLinkerBlazor.Migrations
{
    public partial class RemoveColumnRecordIdInPackageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "PackageForTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecordId",
                table: "PackageForTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

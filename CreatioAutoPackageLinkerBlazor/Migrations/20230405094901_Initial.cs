using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreatioAutoPackageLinkerBlazor.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageForTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordUId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductForTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    ProductForTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_ProductForTypes_ProductForTypeId",
                        column: x => x.ProductForTypeId,
                        principalTable: "ProductForTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfPackageForProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PackageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeOfPackageForProducts_PackageForTypes_PackageId",
                        column: x => x.PackageId,
                        principalTable: "PackageForTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeOfPackageForProducts_ProductForTypes_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductForTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Maintainer = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: true),
                    IsReadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    PackageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PackageUId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    IsModule = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRootPackage = table.Column<bool>(type: "INTEGER", nullable: false),
                    Rang = table.Column<int>(type: "INTEGER", nullable: false),
                    Completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Successfully = table.Column<bool>(type: "INTEGER", nullable: false),
                    ResultDescription = table.Column<string>(type: "TEXT", nullable: true),
                    CanBeRoot = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageHierarchies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BasePackageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DependOnPackageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsModified = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecordInactive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageHierarchies_Packages_BasePackageId",
                        column: x => x.BasePackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageHierarchies_Packages_DependOnPackageId",
                        column: x => x.DependOnPackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageHierarchies_BasePackageId",
                table: "PackageHierarchies",
                column: "BasePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHierarchies_DependOnPackageId",
                table: "PackageHierarchies",
                column: "DependOnPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ProjectId",
                table: "Packages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductForTypeId",
                table: "Projects",
                column: "ProductForTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfPackageForProducts_PackageId",
                table: "TypeOfPackageForProducts",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfPackageForProducts_ProductId",
                table: "TypeOfPackageForProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageHierarchies");

            migrationBuilder.DropTable(
                name: "TypeOfPackageForProducts");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "PackageForTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ProductForTypes");
        }
    }
}

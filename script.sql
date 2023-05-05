CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "PackageForTypes" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "RecordUId" TEXT NOT NULL,
    "RecordId" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    "Name" TEXT NOT NULL
);

CREATE TABLE "ProductForTypes" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "RecordInactive" INTEGER NOT NULL,
    "Name" TEXT NOT NULL
);

CREATE TABLE "Projects" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "Url" TEXT NULL,
    "Login" TEXT NULL,
    "Password" TEXT NULL,
    "ProductForTypeId" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "FK_Projects_ProductForTypes_ProductForTypeId" FOREIGN KEY ("ProductForTypeId") REFERENCES "ProductForTypes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TypeOfPackageForProducts" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "PackageId" TEXT NOT NULL,
    "ProductId" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    CONSTRAINT "FK_TypeOfPackageForProducts_PackageForTypes_PackageId" FOREIGN KEY ("PackageId") REFERENCES "PackageForTypes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TypeOfPackageForProducts_ProductForTypes_ProductId" FOREIGN KEY ("ProductId") REFERENCES "ProductForTypes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Packages" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "CreatedBy" TEXT NULL,
    "CreatedOn" TEXT NULL,
    "Description" TEXT NULL,
    "IsLocked" INTEGER NOT NULL,
    "Maintainer" TEXT NULL,
    "ModifiedBy" TEXT NULL,
    "ModifiedOn" TEXT NULL,
    "Position" INTEGER NULL,
    "IsReadOnly" INTEGER NOT NULL,
    "Type" INTEGER NULL,
    "PackageId" TEXT NOT NULL,
    "PackageUId" TEXT NOT NULL,
    "Version" TEXT NULL,
    "IsModule" INTEGER NOT NULL,
    "IsRootPackage" INTEGER NOT NULL,
    "Rang" INTEGER NOT NULL,
    "Completed" INTEGER NOT NULL,
    "Successfully" INTEGER NOT NULL,
    "ResultDescription" TEXT NULL,
    "CanBeRoot" INTEGER NOT NULL,
    "ProjectId" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "FK_Packages_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PackageHierarchies" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "BasePackageId" TEXT NOT NULL,
    "DependOnPackageId" TEXT NOT NULL,
    "IsModified" INTEGER NOT NULL,
    "IsDelete" INTEGER NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    CONSTRAINT "FK_PackageHierarchies_Packages_BasePackageId" FOREIGN KEY ("BasePackageId") REFERENCES "Packages" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PackageHierarchies_Packages_DependOnPackageId" FOREIGN KEY ("DependOnPackageId") REFERENCES "Packages" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_PackageHierarchies_BasePackageId" ON "PackageHierarchies" ("BasePackageId");

CREATE INDEX "IX_PackageHierarchies_DependOnPackageId" ON "PackageHierarchies" ("DependOnPackageId");

CREATE INDEX "IX_Packages_ProjectId" ON "Packages" ("ProjectId");

CREATE INDEX "IX_Projects_ProductForTypeId" ON "Projects" ("ProductForTypeId");

CREATE INDEX "IX_TypeOfPackageForProducts_PackageId" ON "TypeOfPackageForProducts" ("PackageId");

CREATE INDEX "IX_TypeOfPackageForProducts_ProductId" ON "TypeOfPackageForProducts" ("ProductId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230405094901_Initial', '6.0.15');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "Packages" ADD "ErrorInfo" TEXT NULL;

CREATE TABLE "ef_temp_Projects" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "Login" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "ProductForTypeId" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    "Url" TEXT NOT NULL,
    CONSTRAINT "FK_Projects_ProductForTypes_ProductForTypeId" FOREIGN KEY ("ProductForTypeId") REFERENCES "ProductForTypes" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Projects" ("Id", "Login", "Name", "Password", "ProductForTypeId", "RecordInactive", "Url")
SELECT "Id", IFNULL("Login", ''), "Name", IFNULL("Password", ''), "ProductForTypeId", "RecordInactive", IFNULL("Url", '')
FROM "Projects";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Projects";

ALTER TABLE "ef_temp_Projects" RENAME TO "Projects";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Projects_ProductForTypeId" ON "Projects" ("ProductForTypeId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230405185405_Initial1', '6.0.15');

COMMIT;

BEGIN TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230424075623_ChangeTypeColumnInProject', '6.0.15');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_PackageForTypes" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "RecordInactive" INTEGER NOT NULL,
    "RecordUId" TEXT NOT NULL
);

INSERT INTO "ef_temp_PackageForTypes" ("Id", "Name", "RecordInactive", "RecordUId")
SELECT "Id", "Name", "RecordInactive", "RecordUId"
FROM "PackageForTypes";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "PackageForTypes";

ALTER TABLE "ef_temp_PackageForTypes" RENAME TO "PackageForTypes";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230425144146_RemoveColumnRecordIdInPackageType', '6.0.15');

COMMIT;


START TRANSACTION;

ALTER TABLE "Managers" DROP COLUMN "IsMaster";

ALTER TABLE "Managers" DROP COLUMN "Permissions";

CREATE TABLE "ManagerServiceSupplierRelations" (
    "ManagerId" integer NOT NULL,
    "ServiceSupplierId" integer NOT NULL,
    "ManagerPermissions" integer NOT NULL DEFAULT 2147483647,
    "IsMaster" boolean NOT NULL DEFAULT FALSE,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT "PK_ManagerServiceSupplierRelations" PRIMARY KEY ("ManagerId", "ServiceSupplierId")
);


INSERT INTO "ManagerServiceSupplierRelations" ("ManagerId", "ServiceSupplierId", "ManagerPermissions", "IsMaster", "IsActive")
    SELECT "Id", 1, 2147483647, false, true 
        FROM "Managers";


INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201206050236_AddTableManagerServiceSupplierRelations', '5.0.0');

COMMIT;
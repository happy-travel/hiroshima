START TRANSACTION;

ALTER TABLE "Managers" ADD "IsMaster" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Managers" ADD "Permissions" integer NOT NULL DEFAULT 2147483646;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201130170145_ChangedManagersTableAddedPermissions', '5.0.0');

COMMIT;
START TRANSACTION;

ALTER TABLE "Accommodations" DROP CONSTRAINT "FK_Accommodations_ContractManagers_ContractManagerId";

ALTER TABLE "BookingOrders" DROP CONSTRAINT "FK_BookingOrders_ContractManagers_ContractManagerId";

ALTER TABLE "Contracts" DROP CONSTRAINT "FK_Contracts_ContractManagers_ContractManagerId";

ALTER TABLE "Documents" DROP CONSTRAINT "FK_Documents_ContractManagers_ContractManagerId";

ALTER TABLE "Images" DROP CONSTRAINT "FK_Images_ContractManagers_ContractManagerId";


ALTER TABLE "ContractManagers" DROP CONSTRAINT "PK_ContractManagers";

ALTER TABLE "ContractManagers" RENAME TO "Managers";

ALTER TABLE "Managers" ADD CONSTRAINT "PK_Managers" PRIMARY KEY ("Id");

ALTER INDEX "IX_ContractManagers_Email" RENAME TO "IX_Managers_Email";

ALTER INDEX "IX_ContractManagers_IdentityHash" RENAME TO "IX_Managers_IdentityHash";


ALTER TABLE "Images" RENAME COLUMN "ContractManagerId" TO "ManagerId";

ALTER INDEX "IX_Images_ContractManagerId" RENAME TO "IX_Images_ManagerId";

ALTER TABLE "Documents" RENAME COLUMN "ContractManagerId" TO "ManagerId";

ALTER INDEX "IX_Documents_ContractManagerId" RENAME TO "IX_Documents_ManagerId";

ALTER TABLE "Contracts" RENAME COLUMN "ContractManagerId" TO "ManagerId";

ALTER INDEX "IX_Contracts_ContractManagerId" RENAME TO "IX_Contracts_ManagerId";

ALTER TABLE "BookingOrders" RENAME COLUMN "ContractManagerId" TO "ManagerId";

ALTER INDEX "IX_BookingOrders_ContractManagerId" RENAME TO "IX_BookingOrders_ManagerId";

ALTER TABLE "Accommodations" RENAME COLUMN "ContractManagerId" TO "ManagerId";

ALTER INDEX "IX_Accommodations_ContractManagerId" RENAME TO "IX_Accommodations_ManagerId";


ALTER TABLE "Accommodations" ADD CONSTRAINT "FK_Accommodations_Managers_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES "Managers" ("Id") ON DELETE SET NULL;

ALTER TABLE "BookingOrders" ADD CONSTRAINT "FK_BookingOrders_Managers_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES "Managers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Contracts" ADD CONSTRAINT "FK_Contracts_Managers_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES "Managers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Documents" ADD CONSTRAINT "FK_Documents_Managers_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES "Managers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Images" ADD CONSTRAINT "FK_Images_Managers_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES "Managers" ("Id") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201127204838_RenameTableContractManagers', '5.0.0');

COMMIT;
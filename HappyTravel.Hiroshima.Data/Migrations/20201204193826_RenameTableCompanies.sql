START TRANSACTION;

ALTER TABLE "Accommodations" DROP CONSTRAINT "FK_Accommodations_Companies_CompanyId";

ALTER TABLE "BookingOrders" DROP CONSTRAINT "FK_BookingOrders_Companies_CompanyId";

ALTER TABLE "Contracts" DROP CONSTRAINT "FK_Contracts_Companies_CompanyId";

ALTER TABLE "Documents" DROP CONSTRAINT "FK_Documents_Companies_CompanyId";

ALTER TABLE "Images" DROP CONSTRAINT "FK_Images_Companies_CompanyId";

ALTER TABLE "Managers" DROP CONSTRAINT "FK_Managers_Companies_CompanyId";


ALTER TABLE "Companies" DROP CONSTRAINT "PK_Companies";

ALTER TABLE "Companies" RENAME TO "ServiceSuppliers";

ALTER TABLE "ServiceSuppliers" ADD CONSTRAINT "PK_ServiceSuppliers" PRIMARY KEY ("Id");


ALTER TABLE "Managers" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_Managers_CompanyId" RENAME TO "IX_Managers_ServiceSupplierId";

ALTER TABLE "Images" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_Images_CompanyId" RENAME TO "IX_Images_ServiceSupplierId";

ALTER TABLE "Documents" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_Documents_CompanyId" RENAME TO "IX_Documents_ServiceSupplierId";

ALTER TABLE "Contracts" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_Contracts_CompanyId" RENAME TO "IX_Contracts_ServiceSupplierId";

ALTER TABLE "BookingOrders" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_BookingOrders_CompanyId" RENAME TO "IX_BookingOrders_ServiceSupplierId";

ALTER TABLE "Accommodations" RENAME COLUMN "CompanyId" TO "ServiceSupplierId";

ALTER INDEX "IX_Accommodations_CompanyId" RENAME TO "IX_Accommodations_ServiceSupplierId";


ALTER TABLE "Accommodations" ADD CONSTRAINT "FK_Accommodations_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

ALTER TABLE "BookingOrders" ADD CONSTRAINT "FK_BookingOrders_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Contracts" ADD CONSTRAINT "FK_Contracts_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Documents" ADD CONSTRAINT "FK_Documents_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Images" ADD CONSTRAINT "FK_Images_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

ALTER TABLE "Managers" ADD CONSTRAINT "FK_Managers_ServiceSuppliers_ServiceSupplierId" FOREIGN KEY ("ServiceSupplierId") REFERENCES "ServiceSuppliers" ("Id") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201204193826_RenameTableCompanies', '5.0.0');

COMMIT;
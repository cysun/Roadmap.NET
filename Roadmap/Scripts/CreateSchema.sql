CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Users" (
    "Id" character varying(255) NOT NULL,
    "FirstName" character varying(80) NOT NULL,
    "LastName" character varying(80) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "CampusId" character varying(15),
    "IsVerified" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Users_CampusId" ON "Users" ("CampusId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250813053339_InitialSchema', '9.0.8');

COMMIT;


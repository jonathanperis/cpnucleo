CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "AssignmentTypes" (
        "Id" bytea NOT NULL,
        "Name" text,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_AssignmentTypes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Impediments" (
        "Id" bytea NOT NULL,
        "Name" text,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Impediments" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Organizations" (
        "Id" bytea NOT NULL,
        "Name" text,
        "Description" text,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Organizations" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Users" (
        "Id" bytea NOT NULL,
        "Name" text,
        "Login" text,
        "Password" text,
        "Salt" text,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Workflows" (
        "Id" bytea NOT NULL,
        "Name" text,
        "Order" smallint NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Workflows" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Projects" (
        "Id" bytea NOT NULL,
        "Name" text,
        "OrganizationId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Projects_Organizations_OrganizationId" FOREIGN KEY ("OrganizationId") REFERENCES "Organizations" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Assignments" (
        "Id" bytea NOT NULL,
        "Name" text,
        "Description" text,
        "StartDate" timestamp with time zone NOT NULL,
        "EndDate" timestamp with time zone NOT NULL,
        "AmountHours" smallint NOT NULL,
        "ProjectId" bytea NOT NULL,
        "WorkflowId" bytea NOT NULL,
        "UserId" bytea NOT NULL,
        "AssignmentTypeId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Assignments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Assignments_AssignmentTypes_AssignmentTypeId" FOREIGN KEY ("AssignmentTypeId") REFERENCES "AssignmentTypes" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Assignments_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Assignments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Assignments_Workflows_WorkflowId" FOREIGN KEY ("WorkflowId") REFERENCES "Workflows" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "UserProjects" (
        "Id" bytea NOT NULL,
        "UserId" bytea NOT NULL,
        "ProjectId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_UserProjects" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_UserProjects_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_UserProjects_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "Appointments" (
        "Id" bytea NOT NULL,
        "Description" text,
        "KeepDate" timestamp with time zone NOT NULL,
        "AmountHours" smallint NOT NULL,
        "AssignmentId" bytea NOT NULL,
        "UserId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_Appointments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Appointments_Assignments_AssignmentId" FOREIGN KEY ("AssignmentId") REFERENCES "Assignments" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Appointments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "AssignmentImpediments" (
        "Id" bytea NOT NULL,
        "Description" text,
        "AssignmentId" bytea NOT NULL,
        "ImpedimentId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_AssignmentImpediments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AssignmentImpediments_Assignments_AssignmentId" FOREIGN KEY ("AssignmentId") REFERENCES "Assignments" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AssignmentImpediments_Impediments_ImpedimentId" FOREIGN KEY ("ImpedimentId") REFERENCES "Impediments" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE TABLE "UserAssignments" (
        "Id" bytea NOT NULL,
        "UserId" bytea NOT NULL,
        "AssignmentId" bytea NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        "DeletedAt" timestamp with time zone,
        "Active" boolean NOT NULL,
        CONSTRAINT "PK_UserAssignments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_UserAssignments_Assignments_AssignmentId" FOREIGN KEY ("AssignmentId") REFERENCES "Assignments" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_UserAssignments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Appointments_AssignmentId" ON "Appointments" ("AssignmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Appointments_CreatedAt" ON "Appointments" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Appointments_UserId" ON "Appointments" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_AssignmentImpediments_AssignmentId" ON "AssignmentImpediments" ("AssignmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_AssignmentImpediments_CreatedAt" ON "AssignmentImpediments" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_AssignmentImpediments_ImpedimentId" ON "AssignmentImpediments" ("ImpedimentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Assignments_AssignmentTypeId" ON "Assignments" ("AssignmentTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Assignments_CreatedAt" ON "Assignments" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Assignments_ProjectId" ON "Assignments" ("ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Assignments_UserId" ON "Assignments" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Assignments_WorkflowId" ON "Assignments" ("WorkflowId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_AssignmentTypes_CreatedAt" ON "AssignmentTypes" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Impediments_CreatedAt" ON "Impediments" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Organizations_CreatedAt" ON "Organizations" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Projects_CreatedAt" ON "Projects" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Projects_OrganizationId" ON "Projects" ("OrganizationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserAssignments_AssignmentId" ON "UserAssignments" ("AssignmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserAssignments_CreatedAt" ON "UserAssignments" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserAssignments_UserId" ON "UserAssignments" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserProjects_CreatedAt" ON "UserProjects" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserProjects_ProjectId" ON "UserProjects" ("ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_UserProjects_UserId" ON "UserProjects" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Users_CreatedAt" ON "Users" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    CREATE INDEX "IX_Workflows_CreatedAt" ON "Workflows" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250216012047_InitiaDblMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250216012047_InitiaDblMigration', '8.0.6');
    END IF;
END $EF$;
COMMIT;


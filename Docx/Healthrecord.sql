CREATE TABLE "USERS" (
  "id" integer PRIMARY KEY,
  "firstname" nvarchar,
  "lastname" nvarchar,
  "email" nvarchar,
  "genderid" integer,
  "countyid" integer,
  "countryid" integer,
  "roleid" integer,
  "subcountyid" integer,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "CLIENTS" (
  "id" integer,
  "firstname" nvarchar,
  "surname" nvarchar,
  "middlename" nvarchar,
  "countryid" integer,
  "countyid" integer,
  "subcountyid" integer,
  "genderid" integer,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "PROGRAMS" (
  "id" integer PRIMARY KEY,
  "program" nvarchar,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "ClientPROGRAMS" (
  "id" integer PRIMARY KEY,
  "programid" integer,
  "clientid" integer,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "COUNTRY" (
  "id" integer PRIMARY KEY,
  "countryname" nvarchar,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "COUNTY" (
  "id" integer PRIMARY KEY,
  "countyname" nvarchar,
  "countryid" integer,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "SUBCOUNTY" (
  "id" integer PRIMARY KEY,
  "subcountyname" nvarchar,
  "countyid" integer,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "Roles" (
  "id" integer PRIMARY KEY,
  "rolename" nvarchar,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

CREATE TABLE "gender" (
  "id" integer PRIMARY KEY,
  "gender" nvarchar,
  "createdby" integer,
  "updatedby" integer,
  "createdat" timestamp,
  "updatedat" timestamp
);

ALTER TABLE "USERS" ADD FOREIGN KEY ("roleid") REFERENCES "Roles" ("id");

ALTER TABLE "USERS" ADD FOREIGN KEY ("subcountyid") REFERENCES "SUBCOUNTY" ("id");

ALTER TABLE "CLIENTS" ADD FOREIGN KEY ("countryid") REFERENCES "COUNTRY" ("id");

ALTER TABLE "CLIENTS" ADD FOREIGN KEY ("countyid") REFERENCES "COUNTY" ("id");

ALTER TABLE "CLIENTS" ADD FOREIGN KEY ("subcountyid") REFERENCES "SUBCOUNTY" ("id");

ALTER TABLE "CLIENTS" ADD FOREIGN KEY ("genderid") REFERENCES "gender" ("id");

ALTER TABLE "ClientPROGRAMS" ADD FOREIGN KEY ("programid") REFERENCES "PROGRAMS" ("id");

ALTER TABLE "ClientPROGRAMS" ADD FOREIGN KEY ("clientid") REFERENCES "CLIENTS" ("id");

ALTER TABLE "COUNTY" ADD FOREIGN KEY ("countryid") REFERENCES "COUNTRY" ("id");

ALTER TABLE "SUBCOUNTY" ADD FOREIGN KEY ("countyid") REFERENCES "COUNTRY" ("id");

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                    table.UniqueConstraint("AK_CompanyTypes_Type", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "LenderMatrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tenor = table.Column<int>(type: "int", nullable: false),
                    Spread = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LenderMatrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestedAmount = table.Column<int>(type: "int", nullable: false),
                    MinTenor = table.Column<int>(type: "int", nullable: false),
                    MaxTenor = table.Column<int>(type: "int", nullable: false),
                    BorrowerCompanyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.UniqueConstraint("AK_Permissions_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ReferenceRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FinanceMaxAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FinanceMinAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.UniqueConstraint("AK_Roles_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LoginTries = table.Column<int>(type: "int", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiscalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrowers_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrowers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVerificationAndReset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerificationAndReset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVerificationAndReset_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestedAmount = table.Column<int>(type: "int", nullable: false),
                    RequestedTenor = table.Column<int>(type: "int", nullable: false),
                    FinancePurposeDefinition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BorrowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationStatuses_ApplicationStatusId",
                        column: x => x.ApplicationStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IPO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MarketCapitalization = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShareOutstanding = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    Ticker = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WebUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FinnhubIndustry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BorrowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyProfiles_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedAmount = table.Column<int>(type: "int", nullable: false),
                    ReferenceRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Tenor = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Lenders_LenderId",
                        column: x => x.LenderId,
                        principalTable: "Lenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_LoanStatuses_LoanStatusId",
                        column: x => x.LoanStatusId,
                        principalTable: "LoanStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("23eadac2-3bbb-421b-9a5b-aff07eb74c41"), "Loan Guaranteed" },
                    { new Guid("25275da5-388e-46db-a169-99e1c58d0a7b"), "Loan Disbursed" },
                    { new Guid("287c9a29-9d88-48cb-9aa7-95b33b6fb197"), "Loan Defaulted" },
                    { new Guid("2c656d64-8bcd-4c96-bb96-cd04c231199d"), "Loan Repaid" },
                    { new Guid("2fbde7f8-a9f1-4857-ac10-f818db1dc8b0"), "Loan Rejected" },
                    { new Guid("58749e4c-3bd5-4bf4-b86f-e9b0303a015e"), "Loan Canceled" },
                    { new Guid("b1f0c15d-7c24-4277-991c-5a30a40abacc"), "In Charge" }
                });

            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { new Guid("67d5fd0e-f3a4-4aa7-bef8-8a587bcb475e"), "Description", "Partnership Limited by Shares" },
                    { new Guid("7db974f8-6321-4b35-8ef4-65edda9fe1d6"), "Description", "Sole Proprietorship" },
                    { new Guid("a7eea608-196c-4a52-a5c7-9694e0eb190b"), "Description", "Other" },
                    { new Guid("afbb07dd-70ad-471d-8448-67539b17b872"), "Description", "General Partnership" },
                    { new Guid("b2b5ce14-79b1-402e-92f5-2536bce91dda"), "Description", "Limited Partnership" },
                    { new Guid("be9a7220-0773-4d4d-8ef7-b5fbf480e952"), "Description", "Cooperative Society" }
                });

            migrationBuilder.InsertData(
                table: "Lenders",
                columns: new[] { "Id", "BorrowerCompanyType", "MaxTenor", "MinTenor", "Name", "RequestedAmount" },
                values: new object[,]
                {
                    { new Guid("0f3c377f-89ad-4fd6-af55-62f783b0ea52"), "Sole Proprietorship", 60, 30, "LOGITECH", 100000 },
                    { new Guid("7f83c404-efee-4900-98ee-38d3c95daf56"), "Partnership Limited by Shares", 60, 40, "AZIF", 400000 },
                    { new Guid("8d1ac5ed-0e1e-4de1-a7fc-a7df9e095653"), "Cooperative Society", 65, 30, "PMI BTECH", 100000 }
                });

            migrationBuilder.InsertData(
                table: "LoanStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2dc1c106-6c3d-4af4-98e3-3af497a097f1"), "Erased" },
                    { new Guid("45e0d7e5-0c30-4ee0-b0c6-23379a9bd138"), "Created" },
                    { new Guid("5318a2e8-6a76-49e6-8b2f-97a8e09e5c8c"), "Repaid" },
                    { new Guid("60724bc1-b7ce-4c2a-9663-a0b5b1bd252c"), "Guaranteed" },
                    { new Guid("95269826-ee81-4f28-9783-9bc0d9996300"), "Rejected" },
                    { new Guid("e8db4469-c42e-4cdc-9848-8ae2b7ecccce"), "Defaulted" },
                    { new Guid("f28af2b0-4535-49e7-bb11-00054166a910"), "Disbursed" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("00e181e4-0549-4ebc-8730-77c901bfe676"), null, "CanUpdateLoan" },
                    { new Guid("11b88057-19b4-4fcc-aada-3bdb20877faf"), null, "CanReadCompanyTypes" },
                    { new Guid("2c9dad73-3625-4ecb-9522-c0f59a003e17"), null, "CanReadOwnBorrowers" },
                    { new Guid("2fe0991b-7a0b-4700-8f2c-036782b973bc"), null, "CanCreateMatrix" },
                    { new Guid("37704915-ff65-432f-8767-f5320c0ddea5"), null, "CanReadLoans" },
                    { new Guid("37e38648-4f5d-48ad-9fab-e8dfe5c6e42c"), null, "CanDeleteMatrix" },
                    { new Guid("3843c5cf-6ad5-4920-8bdd-838de39315e2"), null, "CanReadLenders" },
                    { new Guid("3b12b41c-cdd3-45c8-8466-31750b8d3e3c"), null, "CanUpdateApplication" },
                    { new Guid("4d74ce4f-b8d9-4c87-8f48-365c00dc612c"), null, "CanReadApplications" },
                    { new Guid("4f29d160-b6c3-4bff-9bae-d1e6be1dac8b"), null, "CanAddLoan" },
                    { new Guid("52b5a66f-70f9-42e7-aa25-e48171d634a5"), null, "CanReadProducts" },
                    { new Guid("5b3afd8d-f998-455f-b251-0b38b752c663"), null, "CanChangeLoanStatus" },
                    { new Guid("63f2afd8-c671-4dec-93af-de756e2e6e8a"), null, "CanUpdateMatrix" },
                    { new Guid("6fc1cbaf-b307-4efa-8ca9-2cced12a6028"), null, "IsRegistered" },
                    { new Guid("6fe53fc9-8fde-45f2-b3eb-f988e7abd00d"), null, "CanAddApplication" },
                    { new Guid("7ed78266-c462-4c30-8860-dcd405880646"), null, "GenerateEligibles" },
                    { new Guid("91c8ba0c-269b-4462-802c-cb6f3729fc9f"), null, "CanReadStatuses" },
                    { new Guid("943fde30-8f60-4401-a63d-c4218c930882"), null, "CanReadUsers" },
                    { new Guid("96c36a8c-be53-442d-a2f6-b2bcd71b3524"), null, "CanUpdateBorrower" },
                    { new Guid("a333628c-0918-47dd-9c84-30342e0e95e3"), null, "CanReadOwnApplications" },
                    { new Guid("cd978177-aa39-45f5-b6a6-783b9795196c"), null, "CanGenerateMatrix" },
                    { new Guid("d3e011d6-53d4-46c7-8866-840593334476"), null, "CanAddBorrower" },
                    { new Guid("f2a1fba5-a23e-4686-9c0f-3e636b1ac3e4"), null, "CanReadBorrowers" },
                    { new Guid("ff9f62de-ab2a-4ba2-a15d-11a1f214ac66"), null, "IsSuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "FinanceMaxAmount", "FinanceMinAmount", "Name", "ReferenceRate" },
                values: new object[,]
                {
                    { new Guid("5ff6a3be-482e-4826-b027-b7aea05de030"), "Description", 2000000.00m, 10000.00m, "Installments with pre-amortization at a fixed rate", 0.0025m },
                    { new Guid("b2c0e6ae-2a83-4fd3-acce-dd1c647b1b1c"), "Description", 2000000.00m, 10000.00m, "Installment with variable rate pre-amortization", 0.0025m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1afac8e2-d840-40aa-a97f-c3f2bc5931b0"), "SuperAdmin" },
                    { new Guid("846d0436-ffce-49a2-a8ff-bf22aedf0a83"), "RegisteredUser" },
                    { new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0"), "Borrower" },
                    { new Guid("d6013a21-70d7-4c08-9de9-482f339147a8"), "LoanOfficer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsBlocked", "IsEmailConfirmed", "LastName", "LoginTries", "PasswordHash", "PasswordSalt", "PhoneNumber", "Prefix", "Username" },
                values: new object[,]
                {
                    { new Guid("1b2031ff-df77-4ce4-a2f0-00e60546f243"), "kevin.shemili@cardoai.com", "Kevin", false, true, "Shemili", 0, "nsslp9QXF6wOvaGzfIHsoI+23nH+e8+l1SD8bv0IFrI=", "jWRLoRafDBcFS72uPEqyqg==", "683363203", "+355", "kevinshemili1" },
                    { new Guid("75aeebba-0d7d-4f8b-a95d-4d9551167c56"), "kevin.shemili@officer.com", "KevinLoan", false, true, "ShemiliLoan", 0, "nsslp9QXF6wOvaGzfIHsoI+23nH+e8+l1SD8bv0IFrI=", "jWRLoRafDBcFS72uPEqyqg==", "683363203", "+355", "kevinOfficer1" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("ff9f62de-ab2a-4ba2-a15d-11a1f214ac66"), new Guid("1afac8e2-d840-40aa-a97f-c3f2bc5931b0") },
                    { new Guid("6fc1cbaf-b307-4efa-8ca9-2cced12a6028"), new Guid("846d0436-ffce-49a2-a8ff-bf22aedf0a83") },
                    { new Guid("d3e011d6-53d4-46c7-8866-840593334476"), new Guid("846d0436-ffce-49a2-a8ff-bf22aedf0a83") },
                    { new Guid("11b88057-19b4-4fcc-aada-3bdb20877faf"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("2c9dad73-3625-4ecb-9522-c0f59a003e17"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("52b5a66f-70f9-42e7-aa25-e48171d634a5"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("6fe53fc9-8fde-45f2-b3eb-f988e7abd00d"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("96c36a8c-be53-442d-a2f6-b2bcd71b3524"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("a333628c-0918-47dd-9c84-30342e0e95e3"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("d3e011d6-53d4-46c7-8866-840593334476"), new Guid("b05d025a-62ee-4d6c-aef4-9433cc52dcd0") },
                    { new Guid("00e181e4-0549-4ebc-8730-77c901bfe676"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("11b88057-19b4-4fcc-aada-3bdb20877faf"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("2fe0991b-7a0b-4700-8f2c-036782b973bc"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("37704915-ff65-432f-8767-f5320c0ddea5"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("37e38648-4f5d-48ad-9fab-e8dfe5c6e42c"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("3843c5cf-6ad5-4920-8bdd-838de39315e2"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("3b12b41c-cdd3-45c8-8466-31750b8d3e3c"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("4d74ce4f-b8d9-4c87-8f48-365c00dc612c"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("4f29d160-b6c3-4bff-9bae-d1e6be1dac8b"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("52b5a66f-70f9-42e7-aa25-e48171d634a5"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("5b3afd8d-f998-455f-b251-0b38b752c663"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("63f2afd8-c671-4dec-93af-de756e2e6e8a"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("7ed78266-c462-4c30-8860-dcd405880646"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("91c8ba0c-269b-4462-802c-cb6f3729fc9f"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("943fde30-8f60-4401-a63d-c4218c930882"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("cd978177-aa39-45f5-b6a6-783b9795196c"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") },
                    { new Guid("f2a1fba5-a23e-4686-9c0f-3e636b1ac3e4"), new Guid("d6013a21-70d7-4c08-9de9-482f339147a8") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("1afac8e2-d840-40aa-a97f-c3f2bc5931b0"), new Guid("1b2031ff-df77-4ce4-a2f0-00e60546f243") },
                    { new Guid("d6013a21-70d7-4c08-9de9-482f339147a8"), new Guid("75aeebba-0d7d-4f8b-a95d-4d9551167c56") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationStatusId",
                table: "Applications",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_BorrowerId",
                table: "Applications",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ProductId",
                table: "Applications",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowers_CompanyTypeId",
                table: "Borrowers",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowers_UserId",
                table: "Borrowers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_BorrowerId",
                table: "CompanyProfiles",
                column: "BorrowerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ApplicationId",
                table: "Loans",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LenderId",
                table: "Loans",
                column: "LenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanStatusId",
                table: "Loans",
                column: "LoanStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVerificationAndReset_UserEmail",
                table: "UserVerificationAndReset",
                column: "UserEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropTable(
                name: "LenderMatrices");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserVerificationAndReset");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Lenders");

            migrationBuilder.DropTable(
                name: "LoanStatuses");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");

            migrationBuilder.DropTable(
                name: "Borrowers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CompanyTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

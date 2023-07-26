﻿// <auto-generated />
using System;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230726134110_update12")]
    partial class update12
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.ApplicationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BorrowerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FinancePurposeDefinition")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("LoanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RequestedAmount")
                        .HasColumnType("int");

                    b.Property<int>("RequestedTenor")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.HasIndex("LoanId")
                        .IsUnique();

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("Applications", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Borrower", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("FiscalCode")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("VATNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Borrowers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BorrowerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Exchange")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FinnhubIndustry")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IPO")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MarketCapitalization")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("ShareOutstanding")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WebUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId")
                        .IsUnique();

                    b.ToTable("CompanyProfiles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BorrowerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId")
                        .IsUnique();

                    b.ToTable("CompanyTypes", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Lender", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BorrowerCompanyType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RequestedAmount")
                        .HasColumnType("int");

                    b.Property<int>("Tenor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Lenders", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Loan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("InterestRate")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid>("LenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ReferenceRate")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("RequestedAmount")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Tenor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LenderId");

                    b.ToTable("Loans", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("445248ae-bc8b-4a7e-90ce-1636f8206fa5"),
                            Name = "IsSuperAdmin"
                        },
                        new
                        {
                            Id = new Guid("cbf8a920-2ae3-4c26-9bd0-d4a325a25cb3"),
                            Name = "IsRegistered"
                        },
                        new
                        {
                            Id = new Guid("63ec58c3-9fec-432c-bc12-53d312506968"),
                            Name = "CanReadBorrowers"
                        },
                        new
                        {
                            Id = new Guid("09a9bad4-09af-4f79-868d-399372bce957"),
                            Name = "CanAddBorrower"
                        },
                        new
                        {
                            Id = new Guid("7f69557e-8de8-4588-ad25-ad41da56617d"),
                            Name = "CanUpdateBorrower"
                        },
                        new
                        {
                            Id = new Guid("5d29937d-b6a8-4e77-953b-25dce13681ae"),
                            Name = "CanDeleteBorrower"
                        },
                        new
                        {
                            Id = new Guid("c4d09b8a-e67e-40eb-a142-376c3ad6602a"),
                            Name = "CanReadUsers"
                        },
                        new
                        {
                            Id = new Guid("23067061-c25c-4274-8f08-1bc4dfc41a66"),
                            Name = "CanAddUser"
                        },
                        new
                        {
                            Id = new Guid("8833404a-d32a-48f0-8b42-81987e90bc32"),
                            Name = "CanUpdateUser"
                        },
                        new
                        {
                            Id = new Guid("781d185e-7bb1-4c52-8353-f7b3c1b8d886"),
                            Name = "CanDeleteUser"
                        },
                        new
                        {
                            Id = new Guid("8113298f-7f4d-4f45-93a2-c4466adf59ba"),
                            Name = "CanReadApplications"
                        },
                        new
                        {
                            Id = new Guid("569cb42d-5e23-4637-b18e-c50aa6f278d3"),
                            Name = "CanAddApplication"
                        },
                        new
                        {
                            Id = new Guid("64aa8499-4d40-42dc-a958-7eae64598a35"),
                            Name = "CanUpdateApplication"
                        },
                        new
                        {
                            Id = new Guid("4eee49a1-78e0-49c1-bf7b-dd28a8030250"),
                            Name = "CanDeleteApplication"
                        },
                        new
                        {
                            Id = new Guid("d9f453a2-ef19-4c7c-8eac-b496a4f7b41c"),
                            Name = "CanReadLenders"
                        },
                        new
                        {
                            Id = new Guid("f13379d7-a19b-4895-bfd5-fd2da3077f83"),
                            Name = "CanAddLender"
                        },
                        new
                        {
                            Id = new Guid("7510abe2-31fb-4e54-af8a-22450b80284b"),
                            Name = "CanUpdateLender"
                        },
                        new
                        {
                            Id = new Guid("d649c2fe-a286-4858-bf9e-a8ffadd76ccc"),
                            Name = "CanDeleteLender"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("FinanceMaxAmount")
                        .HasColumnType("int");

                    b.Property<int>("FinanceMinAmount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("ReferenceRate")
                        .HasMaxLength(50)
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ProductMatrix", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.ToTable("ProductMatrices", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("6b8f8ee8-d394-487a-847a-cd9e40df4fcf"),
                            Name = "SuperAdmin"
                        },
                        new
                        {
                            Id = new Guid("2b8befb0-b466-4362-bb0b-272c8bdda417"),
                            Name = "LoanOfficerBackOffice"
                        },
                        new
                        {
                            Id = new Guid("c3683837-987d-4be9-a0a6-ea5a6776f21c"),
                            Name = "LoanOfficerFrontOffice"
                        },
                        new
                        {
                            Id = new Guid("bca3d5dc-c4df-4544-a30d-a656eb1916dc"),
                            Name = "Borrower"
                        },
                        new
                        {
                            Id = new Guid("275bb2d0-0b4a-4036-aba9-4103d281a338"),
                            Name = "RegisteredUser"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Role_Permission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("6b8f8ee8-d394-487a-847a-cd9e40df4fcf"),
                            PermissionId = new Guid("445248ae-bc8b-4a7e-90ce-1636f8206fa5")
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("LoginTries")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<bool>("Test")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Username");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("0f7195df-de82-429c-a430-dc0742edf721"),
                            Email = "kevin.shemili@cardoai.com",
                            FirstName = "Kevin",
                            IsBlocked = false,
                            IsEmailConfirmed = true,
                            LastName = "Shemili",
                            LoginTries = 0,
                            PasswordHash = "nsslp9QXF6wOvaGzfIHsoI+23nH+e8+l1SD8bv0IFrI=",
                            PasswordSalt = "jWRLoRafDBcFS72uPEqyqg==",
                            PhoneNumber = "683363203",
                            Prefix = "+355",
                            Test = false,
                            Username = "kevinshemili1"
                        });
                });

            modelBuilder.Entity("Domain.Entities.UserVerificationAndReset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailVerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EmailVerificationTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordResetTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.ToTable("UserVerificationAndReset", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User_Role", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("0f7195df-de82-429c-a430-dc0742edf721"),
                            RoleId = new Guid("6b8f8ee8-d394-487a-847a-cd9e40df4fcf")
                        });
                });

            modelBuilder.Entity("Domain.Entities.ApplicationEntity", b =>
                {
                    b.HasOne("Domain.Entities.Borrower", "Borrower")
                        .WithMany("Applications")
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Loan", "Loan")
                        .WithOne("Application")
                        .HasForeignKey("Domain.Entities.ApplicationEntity", "LoanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.Product", "Product")
                        .WithOne("Application")
                        .HasForeignKey("Domain.Entities.ApplicationEntity", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");

                    b.Navigation("Loan");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entities.Borrower", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Borrowers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.CompanyProfile", b =>
                {
                    b.HasOne("Domain.Entities.Borrower", "Borrower")
                        .WithOne("CompanyProfile")
                        .HasForeignKey("Domain.Entities.CompanyProfile", "BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");
                });

            modelBuilder.Entity("Domain.Entities.CompanyType", b =>
                {
                    b.HasOne("Domain.Entities.Borrower", "Borrower")
                        .WithOne("CompanyType")
                        .HasForeignKey("Domain.Entities.CompanyType", "BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");
                });

            modelBuilder.Entity("Domain.Entities.Loan", b =>
                {
                    b.HasOne("Domain.Entities.Lender", "Lender")
                        .WithMany("Loans")
                        .HasForeignKey("LenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lender");
                });

            modelBuilder.Entity("Domain.Entities.ProductMatrix", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationEntity", "Application")
                        .WithOne("ProductMatrix")
                        .HasForeignKey("Domain.Entities.ProductMatrix", "ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Domain.Entities.Role_Permission", b =>
                {
                    b.HasOne("Domain.Entities.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.UserVerificationAndReset", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne("UserVerificationAndReset")
                        .HasForeignKey("Domain.Entities.UserVerificationAndReset", "UserEmail")
                        .HasPrincipalKey("Domain.Entities.User", "Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.User_Role", b =>
                {
                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.ApplicationEntity", b =>
                {
                    b.Navigation("ProductMatrix");
                });

            modelBuilder.Entity("Domain.Entities.Borrower", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("CompanyProfile")
                        .IsRequired();

                    b.Navigation("CompanyType")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Lender", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Domain.Entities.Loan", b =>
                {
                    b.Navigation("Application")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Product", b =>
                {
                    b.Navigation("Application")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Borrowers");

                    b.Navigation("UserVerificationAndReset")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

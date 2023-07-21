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
    [Migration("20230719135607_update5")]
    partial class update5
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

                    b.ToTable("Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("445248ae-bc8b-4a7e-90ce-1636f8206fa5"),
                            Name = "IsSuperAdmin"
                        },
                        new
                        {
                            Id = new Guid("d95de12a-b2c5-4e7d-81cd-93c89eae0fe4"),
                            Name = "IsRegistered"
                        },
                        new
                        {
                            Id = new Guid("45701c72-40de-4507-b979-024b469f2c10"),
                            Name = "CanReadBorrowers"
                        },
                        new
                        {
                            Id = new Guid("f1408e82-6e08-4f58-9b29-704c0ba40594"),
                            Name = "CanAddBorrower"
                        },
                        new
                        {
                            Id = new Guid("316d231a-2f6a-4be7-88ce-0acc4dcbcdca"),
                            Name = "CanUpdateBorrower"
                        },
                        new
                        {
                            Id = new Guid("7d22805b-c08d-4c67-acb8-cf944487acf2"),
                            Name = "CanDeleteBorrower"
                        },
                        new
                        {
                            Id = new Guid("cf10473f-0a5e-4043-aed0-78a666ed3caa"),
                            Name = "CanReadUsers"
                        },
                        new
                        {
                            Id = new Guid("149a207a-6f31-454d-90c7-c546b7f9efc4"),
                            Name = "CanAddUser"
                        },
                        new
                        {
                            Id = new Guid("e3c94563-8fdf-407b-a6a7-fdeaade72d00"),
                            Name = "CanUpdateUser"
                        },
                        new
                        {
                            Id = new Guid("43597750-002b-4610-ba72-4e1cabf78201"),
                            Name = "CanDeleteUser"
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

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("6b8f8ee8-d394-487a-847a-cd9e40df4fcf"),
                            Name = "SuperAdmin"
                        },
                        new
                        {
                            Id = new Guid("f20386c8-e2f9-4923-8e89-dca0e314f304"),
                            Name = "LoanOfficerBackOffice"
                        },
                        new
                        {
                            Id = new Guid("f79c9e97-568f-4594-b334-be5ded406273"),
                            Name = "LoanOfficerFrontOffice"
                        },
                        new
                        {
                            Id = new Guid("405450de-540f-431e-b527-6a61eb7401b4"),
                            Name = "Borrower"
                        },
                        new
                        {
                            Id = new Guid("28687efb-ba28-49ac-9330-d517e299f87d"),
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

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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
                            IsEmailConfirmed = true,
                            LastName = "Shemili",
                            PasswordHash = "1b7b53ff57d12e357a0c8e3a5f265ecc1867686f9bfa876d3e2cad587086fa45",
                            PasswordSalt = "tU$wPFvTddcR",
                            PhoneNumber = "683363203",
                            Prefix = "+355",
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
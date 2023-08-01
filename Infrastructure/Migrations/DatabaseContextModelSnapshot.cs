﻿// <auto-generated />
using System;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid>("CompanyTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FiscalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("VATNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyTypeId");

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
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Exchange")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FinnhubIndustry")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IPO")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Logo")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal?>("MarketCapitalization")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("ShareOutstanding")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Ticker")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WebUrl")
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

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Type");

                    b.ToTable("CompanyTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("15135cfe-720b-4620-beef-d789ed24883b"),
                            Type = "Sole Proprietorship"
                        },
                        new
                        {
                            Id = new Guid("53ddf09f-1354-4df9-b130-05edb313b09a"),
                            Type = "Other"
                        },
                        new
                        {
                            Id = new Guid("1f2ca40e-31c4-4e2d-b259-c6dfe4fee7e0"),
                            Type = "Partnership Limited by Shares"
                        },
                        new
                        {
                            Id = new Guid("3751947f-8714-4018-9974-414adf397ce7"),
                            Type = "Limited Partnership"
                        },
                        new
                        {
                            Id = new Guid("d97a6931-90a9-4b70-802f-190f8978b081"),
                            Type = "Cooperative Society"
                        },
                        new
                        {
                            Id = new Guid("a2405098-af50-4f9c-a105-7ea6f6998173"),
                            Type = "General Partnership"
                        });
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
                            Id = new Guid("16a36db0-2527-4408-a28c-e52e3d91e17f"),
                            Name = "IsRegistered"
                        },
                        new
                        {
                            Id = new Guid("b4b8511c-e985-4cfe-b397-5ae63e2f7f15"),
                            Name = "CanReadBorrowers"
                        },
                        new
                        {
                            Id = new Guid("5c402131-cd25-40db-b57d-9cf600b36a87"),
                            Name = "CanAddBorrower"
                        },
                        new
                        {
                            Id = new Guid("db3facfb-5dc5-427a-adb0-fc561459a80f"),
                            Name = "CanUpdateBorrower"
                        },
                        new
                        {
                            Id = new Guid("9c527787-f06a-4107-afa5-b72bc8526d21"),
                            Name = "CanDeleteBorrower"
                        },
                        new
                        {
                            Id = new Guid("631297bf-d798-4891-b728-723bc5dfc8d3"),
                            Name = "CanReadUsers"
                        },
                        new
                        {
                            Id = new Guid("e68065a1-4e9e-40cd-b6e3-dfd73723d374"),
                            Name = "CanAddUser"
                        },
                        new
                        {
                            Id = new Guid("0617e228-9c5e-44f5-97b2-c115f7b4a620"),
                            Name = "CanUpdateUser"
                        },
                        new
                        {
                            Id = new Guid("d8a1b354-922f-4476-9b95-1d4fd56374ec"),
                            Name = "CanDeleteUser"
                        },
                        new
                        {
                            Id = new Guid("69a8424a-0e38-4011-ae57-4fda389a6d79"),
                            Name = "CanReadApplications"
                        },
                        new
                        {
                            Id = new Guid("8be3a41c-3290-44e5-acd8-1cc51e60205c"),
                            Name = "CanAddApplication"
                        },
                        new
                        {
                            Id = new Guid("216a3373-f51a-4be3-86b1-e7ff12c966d8"),
                            Name = "CanUpdateApplication"
                        },
                        new
                        {
                            Id = new Guid("c1595c28-1244-4149-a242-2ce49fca854f"),
                            Name = "CanDeleteApplication"
                        },
                        new
                        {
                            Id = new Guid("7ec66ada-3352-49a1-b6ae-6d04d70b9836"),
                            Name = "CanReadLenders"
                        },
                        new
                        {
                            Id = new Guid("faf9218d-b5c7-4cff-b679-1319ad472652"),
                            Name = "CanAddLender"
                        },
                        new
                        {
                            Id = new Guid("f29704d9-97c2-415f-898a-5ce6c662a3b8"),
                            Name = "CanUpdateLender"
                        },
                        new
                        {
                            Id = new Guid("8de77e5f-3ca9-4e3e-aea4-ef2f03c02a54"),
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
                            Id = new Guid("c3f6395e-1180-4d98-b6bd-d0af0a1d0612"),
                            Name = "LoanOfficerBackOffice"
                        },
                        new
                        {
                            Id = new Guid("4604b7ef-d888-4e45-9383-2aefa82c8ad1"),
                            Name = "LoanOfficerFrontOffice"
                        },
                        new
                        {
                            Id = new Guid("09aaf939-2678-4707-88b7-ec76b53393a6"),
                            Name = "Borrower"
                        },
                        new
                        {
                            Id = new Guid("8ddab6b4-be14-4fe1-b491-a32051941753"),
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
                    b.HasOne("Domain.Entities.CompanyType", "CompanyType")
                        .WithMany("Borrowers")
                        .HasForeignKey("CompanyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Borrowers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyType");

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
                });

            modelBuilder.Entity("Domain.Entities.CompanyType", b =>
                {
                    b.Navigation("Borrowers");
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

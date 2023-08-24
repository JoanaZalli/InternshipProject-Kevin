using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("11b88057-19b4-4fcc-aada-3bdb20877faf"), new Guid("846d0436-ffce-49a2-a8ff-bf22aedf0a83") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11b88057-19b4-4fcc-aada-3bdb20877faf"), new Guid("846d0436-ffce-49a2-a8ff-bf22aedf0a83") });
        }
    }
}

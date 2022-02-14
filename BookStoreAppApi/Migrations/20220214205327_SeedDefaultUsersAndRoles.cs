using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAppApi.Migrations
{
    public partial class SeedDefaultUsersAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9539a969-975b-41b1-9aa8-0879efebc322", "d08e7d2d-95ac-479d-a387-59b4bc852c28", "User", "USER" },
                    { "fe7c3b95-8c2b-4886-9b01-e388061add34", "e9755df5-63cc-4e0f-a7c1-69bdf5f7b7e5", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1b98fd57-a180-4f64-b08f-35dc8f7f7986", 0, "0b6793e0-0c29-4302-ba6b-f3e2c1ef97b2", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEPbtQazXrIuGkC0NZIXd+xh/aja1D3MuHr+scoO5/ZbkbfL4TkttdMHOjN9Q3w3WWg==", null, false, "78f0bb85-6758-4f6f-9a86-8a5365eff1b6", false, "admin@bookstore.com" },
                    { "a235814b-ec8f-4d7b-b543-e9587402ad9b", 0, "16675a23-2f19-4115-b4bc-79b0e835fbf3", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAENpxCgBYVlCPL1dGrXUsfjV7m1CkmaMbgCILaDC0CBUqcx8kWkVf5o9PM53T5RS5SQ==", null, false, "a5189389-c99d-4226-830a-7d7bb8d7fa4c", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fe7c3b95-8c2b-4886-9b01-e388061add34", "1b98fd57-a180-4f64-b08f-35dc8f7f7986" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9539a969-975b-41b1-9aa8-0879efebc322", "a235814b-ec8f-4d7b-b543-e9587402ad9b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fe7c3b95-8c2b-4886-9b01-e388061add34", "1b98fd57-a180-4f64-b08f-35dc8f7f7986" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9539a969-975b-41b1-9aa8-0879efebc322", "a235814b-ec8f-4d7b-b543-e9587402ad9b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9539a969-975b-41b1-9aa8-0879efebc322");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe7c3b95-8c2b-4886-9b01-e388061add34");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1b98fd57-a180-4f64-b08f-35dc8f7f7986");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a235814b-ec8f-4d7b-b543-e9587402ad9b");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class AccountStatusTypesEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: new Guid("780c4083-a30a-4789-8d1b-9eb95d1122ff"));

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "Id", "AccountId", "Amount", "Currency", "Description", "RunningBalance", "TransactionDate", "TransactionType" },
                values: new object[] { new Guid("b3f53110-f85c-4a7b-ad82-c42a1903ecc5"), new Guid("33333333-3333-3333-3333-333333333333"), 10000m, "JOD", "Initial opening balance", 10000m, new DateTime(2026, 2, 26, 0, 14, 15, 139, DateTimeKind.Utc).AddTicks(1391), "Deposit" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$qypjwhTZlI4wnAMJTPnXzOqogYSFAgYjNOWgtuF9e6P5BE1YCTnaK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "transactions",
                keyColumn: "Id",
                keyValue: new Guid("b3f53110-f85c-4a7b-ad82-c42a1903ecc5"));

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "Id", "AccountId", "Amount", "Currency", "Description", "RunningBalance", "TransactionDate", "TransactionType" },
                values: new object[] { new Guid("780c4083-a30a-4789-8d1b-9eb95d1122ff"), new Guid("33333333-3333-3333-3333-333333333333"), 10000m, "JOD", "Initial opening balance", 10000m, new DateTime(2025, 12, 22, 20, 23, 33, 618, DateTimeKind.Utc).AddTicks(1468), "Deposit" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$q2oe6LQHWl9ssLrYLfJOfOmpoGRHjjaddxX/LicJAZKgtjUBEiD2u");
        }
    }
}

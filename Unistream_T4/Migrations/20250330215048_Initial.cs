using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unistream_T4.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientBalances",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    LastCalculationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBalances", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TimeZoneOffset = table.Column<TimeSpan>(type: "interval", nullable: false),
                    InsertDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientBalanceAfterAddTransaction = table.Column<decimal>(type: "numeric", nullable: true),
                    IsReverted = table.Column<bool>(type: "boolean", nullable: false),
                    RevertDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientBalanceAfterRevertTransaction = table.Column<decimal>(type: "numeric", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBalances");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}

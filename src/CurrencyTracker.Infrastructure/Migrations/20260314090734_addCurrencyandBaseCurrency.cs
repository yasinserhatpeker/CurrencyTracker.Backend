using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCurrencyandBaseCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuoteCurrency",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BaseCurrency",
                table: "Portfolios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuoteCurrency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BaseCurrency",
                table: "Portfolios");
        }
    }
}

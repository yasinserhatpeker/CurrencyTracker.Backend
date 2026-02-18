using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRefreshTokenHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenHash",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenHash",
                table: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordTokenHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordToken",
                table: "Users",
                newName: "ResetPasswordTokenHash");

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordTokenHash",
                table: "Users",
                newName: "ResetPasswordToken");

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

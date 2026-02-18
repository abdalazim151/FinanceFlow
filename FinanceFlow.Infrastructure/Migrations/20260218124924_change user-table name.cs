using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeusertablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_DomainUsers_UserId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DomainUsers",
                table: "DomainUsers");

            migrationBuilder.RenameTable(
                name: "DomainUsers",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "DomainUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DomainUsers",
                table: "DomainUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_DomainUsers_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "DomainUsers",
                principalColumn: "Id");
        }
    }
}

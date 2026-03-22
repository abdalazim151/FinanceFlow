using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lastISA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AtmMachines_AtmMachineId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_User1Id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_User2Id",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_User2Id",
                table: "Transaction",
                newName: "IX_Transaction_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_User1Id",
                table: "Transaction",
                newName: "IX_Transaction_User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AtmMachineId",
                table: "Transaction",
                newName: "IX_Transaction_AtmMachineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AtmMachines_AtmMachineId",
                table: "Transaction",
                column: "AtmMachineId",
                principalTable: "AtmMachines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Users_User1Id",
                table: "Transaction",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Users_User2Id",
                table: "Transaction",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AtmMachines_AtmMachineId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Users_User1Id",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Users_User2Id",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_User2Id",
                table: "Transactions",
                newName: "IX_Transactions_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_User1Id",
                table: "Transactions",
                newName: "IX_Transactions_User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_AtmMachineId",
                table: "Transactions",
                newName: "IX_Transactions_AtmMachineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AtmMachines_AtmMachineId",
                table: "Transactions",
                column: "AtmMachineId",
                principalTable: "AtmMachines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_User1Id",
                table: "Transactions",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_User2Id",
                table: "Transactions",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

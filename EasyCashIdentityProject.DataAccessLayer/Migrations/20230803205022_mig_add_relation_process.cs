using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyCashIdentityProject.DataAccessLayer.Migrations
{
    public partial class mig_add_relation_process : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditorID",
                table: "CustomerAccountProcesses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DebitorID",
                table: "CustomerAccountProcesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccountProcesses_CreditorID",
                table: "CustomerAccountProcesses",
                column: "CreditorID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccountProcesses_DebitorID",
                table: "CustomerAccountProcesses",
                column: "DebitorID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccountProcesses_CustomerAccounts_CreditorID",
                table: "CustomerAccountProcesses",
                column: "CreditorID",
                principalTable: "CustomerAccounts",
                principalColumn: "CustomerAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccountProcesses_CustomerAccounts_DebitorID",
                table: "CustomerAccountProcesses",
                column: "DebitorID",
                principalTable: "CustomerAccounts",
                principalColumn: "CustomerAccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccountProcesses_CustomerAccounts_CreditorID",
                table: "CustomerAccountProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccountProcesses_CustomerAccounts_DebitorID",
                table: "CustomerAccountProcesses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccountProcesses_CreditorID",
                table: "CustomerAccountProcesses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccountProcesses_DebitorID",
                table: "CustomerAccountProcesses");

            migrationBuilder.DropColumn(
                name: "CreditorID",
                table: "CustomerAccountProcesses");

            migrationBuilder.DropColumn(
                name: "DebitorID",
                table: "CustomerAccountProcesses");
        }
    }
}

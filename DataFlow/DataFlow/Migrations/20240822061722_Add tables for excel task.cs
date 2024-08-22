using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataFlow.Migrations
{
    /// <inheritdoc />
    public partial class Addtablesforexceltask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadedFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialClasses_LoadedFiles_LoadedFileId",
                        column: x => x.LoadedFileId,
                        principalTable: "LoadedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveOpenningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PassiveOpenningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebitTurnover = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditTurnover = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActiveClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PassiveClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_FinancialClasses_FinancialClassId",
                        column: x => x.FinancialClassId,
                        principalTable: "FinancialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_FinancialClassId",
                table: "BankAccounts",
                column: "FinancialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialClasses_LoadedFileId",
                table: "FinancialClasses",
                column: "LoadedFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "FinancialClasses");

            migrationBuilder.DropTable(
                name: "LoadedFiles");
        }
    }
}

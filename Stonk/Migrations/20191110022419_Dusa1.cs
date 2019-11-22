using Microsoft.EntityFrameworkCore.Migrations;

namespace Stonk.Migrations
{
    public partial class Dusa1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isAdmin = table.Column<bool>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company = table.Column<string>(nullable: true),
                    stockCode = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    firstValue = table.Column<decimal>(nullable: false),
                    growTrend = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isAdmin = table.Column<bool>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StockValuesInTime",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stockId = table.Column<int>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    timestamp = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockValuesInTime", x => x.id);
                    table.ForeignKey(
                        name: "FK_StockValuesInTime_Stock_stockId",
                        column: x => x.stockId,
                        principalTable: "Stock",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(nullable: false),
                    cash = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    portfolioId = table.Column<int>(nullable: false),
                    stockId = table.Column<int>(nullable: false),
                    amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => x.id);
                    table.ForeignKey(
                        name: "FK_Share_Portfolios_portfolioId",
                        column: x => x.portfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    portfolioId = table.Column<int>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    verified = table.Column<bool>(nullable: false),
                    cash = table.Column<decimal>(nullable: false),
                    assets = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transactions_Portfolios_portfolioId",
                        column: x => x.portfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_userId",
                table: "Portfolios",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Share_portfolioId",
                table: "Share",
                column: "portfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockValuesInTime_stockId",
                table: "StockValuesInTime",
                column: "stockId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_portfolioId",
                table: "Transactions",
                column: "portfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.DropTable(
                name: "StockValuesInTime");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

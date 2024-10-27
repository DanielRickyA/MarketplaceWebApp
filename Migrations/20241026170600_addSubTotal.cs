using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addSubTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubTotal",
                table: "Keranjang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keranjang_IdCustomer",
                table: "Keranjang",
                column: "IdCustomer");

            migrationBuilder.AddForeignKey(
                name: "FK_Keranjang_Customer_IdCustomer",
                table: "Keranjang",
                column: "IdCustomer",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keranjang_Customer_IdCustomer",
                table: "Keranjang");

            migrationBuilder.DropIndex(
                name: "IX_Keranjang_IdCustomer",
                table: "Keranjang");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Keranjang");
        }
    }
}

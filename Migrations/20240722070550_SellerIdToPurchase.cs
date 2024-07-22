using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMVC.Migrations
{
    /// <inheritdoc />
    public partial class SellerIdToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "PurchasedItems",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "PurchasedItems");
        }
    }
}

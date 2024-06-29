using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePro.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeTokenToBaymentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeToken",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeToken",
                table: "payments");
        }
    }
}

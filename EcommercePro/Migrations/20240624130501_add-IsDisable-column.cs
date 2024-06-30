using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePro.Migrations
{
    /// <inheritdoc />
    public partial class addIsDisablecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "AspNetUsers");
        }
    }
}

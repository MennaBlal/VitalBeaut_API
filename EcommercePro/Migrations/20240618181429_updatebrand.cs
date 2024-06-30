using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePro.Migrations
{
    /// <inheritdoc />
    public partial class updatebrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotifications");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Brands");

            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commercialRegistrationImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.Id);
                });
        }
    }
}

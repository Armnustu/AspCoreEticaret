using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahinTürkEticaretCore.Migrations
{
    /// <inheritdoc />
    public partial class DeleteEmailOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "vm_MyOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "vm_MyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

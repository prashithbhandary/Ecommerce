using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clothing_store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatingproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "rating",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "Products");
        }
    }
}

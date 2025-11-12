using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadDirecter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomProperties",
                table: "Leads",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomProperties",
                table: "Leads");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2BCorp.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class SchemaChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelinquent",
                table: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelinquent",
                table: "Invoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}

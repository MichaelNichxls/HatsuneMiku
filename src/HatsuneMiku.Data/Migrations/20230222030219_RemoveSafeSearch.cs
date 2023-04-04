using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSafeSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SafeSearch",
                table: "ImageQueries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SafeSearch",
                table: "ImageQueries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

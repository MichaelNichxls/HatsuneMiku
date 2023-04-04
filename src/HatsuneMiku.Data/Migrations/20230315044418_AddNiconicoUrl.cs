using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNiconicoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NiconicoUrl",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NiconicoUrl",
                table: "Songs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageResultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageQueryResults_ImageResultId",
                table: "ImageQueryResults");

            migrationBuilder.CreateIndex(
                name: "IX_ImageQueryResults_ImageResultId",
                table: "ImageQueryResults",
                column: "ImageResultId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageQueryResults_ImageResultId",
                table: "ImageQueryResults");

            migrationBuilder.CreateIndex(
                name: "IX_ImageQueryResults_ImageResultId",
                table: "ImageQueryResults",
                column: "ImageResultId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageQueryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageQueryEntityId",
                table: "ImageResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    SafeSearchLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageQueries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageResults_ImageQueryEntityId",
                table: "ImageResults",
                column: "ImageQueryEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResults_ImageQueries_ImageQueryEntityId",
                table: "ImageResults",
                column: "ImageQueryEntityId",
                principalTable: "ImageQueries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResults_ImageQueries_ImageQueryEntityId",
                table: "ImageResults");

            migrationBuilder.DropTable(
                name: "ImageQueries");

            migrationBuilder.DropIndex(
                name: "IX_ImageResults_ImageQueryEntityId",
                table: "ImageResults");

            migrationBuilder.DropColumn(
                name: "ImageQueryEntityId",
                table: "ImageResults");
        }
    }
}

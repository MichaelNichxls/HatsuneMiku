using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageQueryResultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResults_ImageQueries_ImageQueryEntityId",
                table: "ImageResults");

            migrationBuilder.DropIndex(
                name: "IX_ImageResults_ImageQueryEntityId",
                table: "ImageResults");

            migrationBuilder.DropColumn(
                name: "ImageQueryEntityId",
                table: "ImageResults");

            migrationBuilder.CreateTable(
                name: "ImageQueryResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageQueryId = table.Column<int>(type: "int", nullable: false),
                    ImageResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageQueryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageQueryResults_ImageQueries_ImageQueryId",
                        column: x => x.ImageQueryId,
                        principalTable: "ImageQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageQueryResults_ImageResults_ImageResultId",
                        column: x => x.ImageResultId,
                        principalTable: "ImageResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageQueryResults_ImageQueryId",
                table: "ImageQueryResults",
                column: "ImageQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageQueryResults_ImageResultId",
                table: "ImageQueryResults",
                column: "ImageResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageQueryResults");

            migrationBuilder.AddColumn<int>(
                name: "ImageQueryEntityId",
                table: "ImageResults",
                type: "int",
                nullable: true);

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
    }
}

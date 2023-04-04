using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImageEntityOverhaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageQueryResults");

            migrationBuilder.DropTable(
                name: "ImageQueries");

            migrationBuilder.DropTable(
                name: "ImageResults");

            migrationBuilder.CreateTable(
                name: "BraveImageQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SafeSearch = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Layout = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    License = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BraveImageQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BraveImageResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageAge = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResizedUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BraveImageResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuckDuckGoImageQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SafeSearch = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Layout = table.Column<int>(type: "int", nullable: false),
                    License = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuckDuckGoImageQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuckDuckGoImageResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuckDuckGoImageResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleImageQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    SafeSearch = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleImageQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleImageResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: true),
                    DisplayUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleImageResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BraveImageQueryResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BraveImageQueryId = table.Column<int>(type: "int", nullable: false),
                    BraveImageResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BraveImageQueryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BraveImageQueryResults_BraveImageQueries_BraveImageQueryId",
                        column: x => x.BraveImageQueryId,
                        principalTable: "BraveImageQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BraveImageQueryResults_BraveImageResults_BraveImageResultId",
                        column: x => x.BraveImageResultId,
                        principalTable: "BraveImageResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DuckDuckGoImageQueryResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuckDuckGoImageQueryId = table.Column<int>(type: "int", nullable: false),
                    DuckDuckGoImageResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuckDuckGoImageQueryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuckDuckGoImageQueryResults_DuckDuckGoImageQueries_DuckDuckGoImageQueryId",
                        column: x => x.DuckDuckGoImageQueryId,
                        principalTable: "DuckDuckGoImageQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuckDuckGoImageQueryResults_DuckDuckGoImageResults_DuckDuckGoImageResultId",
                        column: x => x.DuckDuckGoImageResultId,
                        principalTable: "DuckDuckGoImageResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoogleImageQueryResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleImageQueryId = table.Column<int>(type: "int", nullable: false),
                    GoogleImageResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleImageQueryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleImageQueryResults_GoogleImageQueries_GoogleImageQueryId",
                        column: x => x.GoogleImageQueryId,
                        principalTable: "GoogleImageQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoogleImageQueryResults_GoogleImageResults_GoogleImageResultId",
                        column: x => x.GoogleImageResultId,
                        principalTable: "GoogleImageResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BraveImageQueryResults_BraveImageQueryId",
                table: "BraveImageQueryResults",
                column: "BraveImageQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_BraveImageQueryResults_BraveImageResultId",
                table: "BraveImageQueryResults",
                column: "BraveImageResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuckDuckGoImageQueryResults_DuckDuckGoImageQueryId",
                table: "DuckDuckGoImageQueryResults",
                column: "DuckDuckGoImageQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_DuckDuckGoImageQueryResults_DuckDuckGoImageResultId",
                table: "DuckDuckGoImageQueryResults",
                column: "DuckDuckGoImageResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoogleImageQueryResults_GoogleImageQueryId",
                table: "GoogleImageQueryResults",
                column: "GoogleImageQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleImageQueryResults_GoogleImageResultId",
                table: "GoogleImageQueryResults",
                column: "GoogleImageResultId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BraveImageQueryResults");

            migrationBuilder.DropTable(
                name: "DuckDuckGoImageQueryResults");

            migrationBuilder.DropTable(
                name: "GoogleImageQueryResults");

            migrationBuilder.DropTable(
                name: "BraveImageQueries");

            migrationBuilder.DropTable(
                name: "BraveImageResults");

            migrationBuilder.DropTable(
                name: "DuckDuckGoImageQueries");

            migrationBuilder.DropTable(
                name: "DuckDuckGoImageResults");

            migrationBuilder.DropTable(
                name: "GoogleImageQueries");

            migrationBuilder.DropTable(
                name: "GoogleImageResults");

            migrationBuilder.CreateTable(
                name: "ImageQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageResults", x => x.Id);
                });

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
                column: "ImageResultId",
                unique: true);
        }
    }
}

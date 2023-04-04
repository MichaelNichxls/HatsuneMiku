using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSongEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WikiUrl",
                table: "Songs",
                newName: "Producer");

            migrationBuilder.AddColumn<string>(
                name: "SoundCloudUrl",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoundCloudUrl",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "Producer",
                table: "Songs",
                newName: "WikiUrl");
        }
    }
}

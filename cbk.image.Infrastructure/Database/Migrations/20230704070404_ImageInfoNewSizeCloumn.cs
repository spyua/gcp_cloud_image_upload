using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cbk.image.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageInfoNewSizeCloumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Size",
                table: "ImageInformations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ImageInformations");
        }
    }
}

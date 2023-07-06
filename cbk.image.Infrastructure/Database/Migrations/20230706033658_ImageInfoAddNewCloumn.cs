using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cbk.image.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageInfoAddNewCloumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaLink",
                table: "ImageInformations",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaLink",
                table: "ImageInformations");
        }
    }
}

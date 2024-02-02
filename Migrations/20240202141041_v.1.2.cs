using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmRecensioner.Migrations
{
    /// <inheritdoc />
    public partial class v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomUserId",
                table: "Reviews",
                column: "CustomUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomUserId",
                table: "Reviews",
                column: "CustomUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CustomUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CustomUserId",
                table: "Reviews");
        }
    }
}

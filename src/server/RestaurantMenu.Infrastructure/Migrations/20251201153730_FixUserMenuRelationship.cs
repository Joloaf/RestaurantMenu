using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantMenu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserMenuRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_UserId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId1",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Menus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId1",
                table: "Menus",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_UserId1",
                table: "Menus",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

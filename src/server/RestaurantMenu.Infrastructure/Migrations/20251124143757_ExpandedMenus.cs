using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantMenu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Menus_MenuId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MenuId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Menus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Menus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId",
                table: "Menus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId1",
                table: "Menus",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_UserId",
                table: "Menus",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_UserId1",
                table: "Menus",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_UserId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_UserId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId1",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MenuId",
                table: "AspNetUsers",
                column: "MenuId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Menus_MenuId",
                table: "AspNetUsers",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
